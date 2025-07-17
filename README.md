# API Explorateur d'Ouvertures

Ce projet fournit une API Web ASP.NET Core minimale pour explorer les ouvertures d'échecs. Elle expose des points de terminaison sécurisés par authentification JWT et stocke les données dans une base SQLite. Une petite interface HTML/JavaScript est incluse dans `wwwroot`.

## Fonctionnalités

- Inscription et connexion de l'utilisateur renvoyant un jeton JWT.
- Opérations CRUD sur les ouvertures d'échecs.
- Migrations automatiques de la base au démarrage.
- Swagger UI activé pour la documentation interactive.
- Données d'exemple et un utilisateur de démonstration (`demo` / `Pass!234`) créés lors de la première exécution.

## Prérequis

- [SDK .NET 8](https://dotnet.microsoft.com/) installé.

Les modules Node utilisés par le front‑end sont déjà inclus dans le dépôt ; aucune installation supplémentaire n'est nécessaire pour un démarrage rapide.

## Exécuter l'application

```bash
# restaurer les packages et lancer l'API
dotnet run
```

Par défaut, l'API écoute sur `http://localhost:5107` (voir `Properties/launchSettings.json`). Lors du démarrage, l'application applique les migrations en attente et crée la base SQLite `openingexplorer.db` si elle n'existe pas.

Rendez‑vous sur `/swagger` pour consulter la documentation interactive ou ouvrez `http://localhost:5107` dans votre navigateur pour charger le front‑end contenu dans `wwwroot`.

### Exemple de workflow

1. Démarrez l'API avec `dotnet run`.
2. Utilisez les identifiants de démonstration (`demo` / `Pass!234`) pour vous authentifier via `/api/auth/login`.
3. Envoyez des requêtes à `/api/openings` avec le jeton obtenu (voir Swagger UI pour les détails).

## Structure du projet

- `Controllers/` – Endpoints pour l'authentification et les ouvertures.
- `Services/` – Logique métier et accès aux données via Entity Framework Core.
- `Entities/` – Classes d'entités de la base de données.
- `DTOs/` – Objets de transfert de données utilisés par l'API.
- `Data/` – `DbContext` d'Entity Framework Core.
- `wwwroot/` – Front‑end simple utilisant HTML, CSS et JavaScript.

## Configuration

Les paramètres JWT et la chaîne de connexion SQLite sont stockés dans `appsettings.json`. Ajustez-les si besoin avant un déploiement en production.

---
Ce README est fourni en réponse à la demande de l'utilisateur d'ajouter de la documentation au dépôt.
