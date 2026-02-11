
# Chess Orchestre (Opening Explorer) - Guide de Déploiement Kubernetes/OpenShift

## Architecture

```
                    ┌─────────────────────────────────────────┐
                    │          OpenShift Cluster               │
                    │                                         │
  Internet ──►  Route (edge TLS)                              │
                    │                                         │
    chess.apps.kakor.ovh ──► app-svc ──► Opening Explorer     │
                    │        (8080)      (.NET 8, x1 pod)     │
                    │                                         │
                    │   L'app sert le frontend (wwwroot/)     │
                    │   + l'API REST + Swagger                │
                    │                                         │
                    │   Volume ──► PVC ──► PV (NFS)           │
                    │   /app/data   192.168.1.56:/...gp-5/db  │
                    └─────────────────────────────────────────┘
```

> **Note :** L'application est un monolithe .NET 8 qui sert à la fois le frontend statique (HTML/CSS/JS depuis `wwwroot/`) et l'API REST. Un seul Deployment suffit.

## Stack technique

| Composant | Technologie |
|-----------|-------------|
| Backend   | ASP.NET Core 8 (Minimal API + Controllers) |
| Frontend  | HTML/CSS/JS vanilla (servi depuis `wwwroot/`) |
| Base de données | SQLite (`openingexplorer.db`) |
| Auth      | JWT Bearer (issuer: `OpeningExplorerAPI`) |
| Visualisation | Chart.js, vue3-chessboard |
| Doc API   | Swagger / OpenAPI |

## Prérequis

- Docker installé
- .NET 8 SDK (pour le build local)
- Accès à harbor.kakor.ovh (login)
- kubectl/oc CLI configuré vers le cluster OpenShift

## Lancement local

```bash
# Restaurer les dépendances et lancer
dotnet run

# L'app est disponible sur :
# - http://localhost:5107        (frontend + API)
# - http://localhost:5107/swagger (documentation API)

# Compte démo créé automatiquement : demo / Pass!234
```

## Étape 1 : Build et Push de l'image Docker

Le `Dockerfile` multi-stage est déjà à la racine du projet.

### Login Harbor et push

```bash
docker login harbor.kakor.ovh

docker build -t harbor.kakor.ovh/ipi/project-gp-5:latest .
docker push harbor.kakor.ovh/ipi/project-gp-5:latest
```

## Étape 2 : Préparer le NFS

Sur le serveur NFS, créer le répertoire pour la base SQLite :
```bash
mkdir -p /Volume1/public/nfs-share-openshift/project-gp-5/db
chmod 777 /Volume1/public/nfs-share-openshift/project-gp-5/db
```

## Étape 3 : Déployer sur Kubernetes/OpenShift

Appliquer les manifests dans l'ordre :
```bash
# Secrets (clé JWT)
kubectl apply -f k8s/01-secrets.yaml

# PV et PVC pour SQLite (stockage NFS persistant)
kubectl apply -f k8s/02-pv-db.yaml

# Application Deployment + Service
kubectl apply -f k8s/03-app-deployment.yaml

# Attendre que l'app soit prête
kubectl -n project-gp-5 wait --for=condition=available deployment/opening-explorer --timeout=120s

# Route OpenShift (Edge TLS)
kubectl apply -f k8s/04-ingress-routes.yaml
```

Ou tout en une commande :
```bash
kubectl apply -f k8s/
```

## Étape 4 : Vérification

```bash
# Vérifier les pods
kubectl -n project-gp-5 get pods

# Vérifier les services
kubectl -n project-gp-5 get svc

# Vérifier les routes
kubectl -n project-gp-5 get routes

# Logs de l'application
kubectl -n project-gp-5 logs -l app=opening-explorer

# Test de l'API
kubectl -n project-gp-5 exec -it deploy/opening-explorer -- curl -s http://localhost:8080/swagger/v1/swagger.json | head -20
```

## URLs d'accès

| Composant | URL |
|-----------|-----|
| Application (frontend + API) | https://chess.apps.kakor.ovh |
| Swagger | https://chess.apps.kakor.ovh/swagger |

## Endpoints API

| Méthode | Route | Description | Auth |
|---------|-------|-------------|------|
| POST | `/api/auth/register` | Inscription | Non |
| POST | `/api/auth/login` | Connexion (retourne un JWT) | Non |
| GET | `/api/openings` | Lister les ouvertures | Oui |
| GET | `/api/openings/{id}` | Détail d'une ouverture | Oui |
| POST | `/api/openings` | Créer une ouverture | Oui |
| PUT | `/api/openings/{id}` | Modifier une ouverture | Oui |
| DELETE | `/api/openings/{id}` | Supprimer une ouverture | Oui |

## Points de sécurité

- L'image Docker ne tourne **pas en root** (user `appuser`)
- La clé JWT est stockée dans un **Secret Kubernetes** (pas en clair dans les Deployments)
- La route utilise **TLS edge** (HTTPS) avec redirection HTTP → HTTPS
- La base SQLite est persistée sur un **volume NFS** monté en `/app/data`

## Variables de configuration

Pour la production, surcharger via des variables d'environnement ou un ConfigMap :

```yaml
env:
  - name: ConnectionStrings__Default
    value: "Data Source=/app/data/openingexplorer.db"
  - name: Jwt__Key
    valueFrom:
      secretKeyRef:
        name: project-gp-5-secrets
        key: jwt-key
  - name: Jwt__Issuer
    value: "OpeningExplorerAPI"
  - name: Jwt__Audience
    value: "OpeningExplorerClient"
```
