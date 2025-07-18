# ChessAPI

Ce projet propose une petite API REST permettant de gérer des ouvertures d'échecs.

## Lancer l'application

```bash
dotnet run
```

Une fois démarrée, la documentation Swagger est disponible à l'adresse `http://localhost:5000/swagger` (ou le port configuré).

## Endpoints principaux

- `POST /api/auth/register` : création d'un compte utilisateur
- `POST /api/auth/login` : obtention d'un jeton JWT
- `GET /api/openings` : liste des ouvertures (authentification requise)
- `GET /api/openings/{id}` : détails d'une ouverture (authentification requise)
- `POST /api/openings` : création d'une ouverture (authentification requise)
- `PUT /api/openings/{id}` : mise à jour d'une ouverture (authentification requise)
- `DELETE /api/openings/{id}` : suppression d'une ouverture (authentification requise)

Chaque endpoint est décrit plus en détail dans l'interface Swagger.
