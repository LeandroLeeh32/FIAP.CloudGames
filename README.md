# FIAP.CloudGames

API REST para gerenciamento de usuarios, jogos e promocoes, com autenticacao JWT, persistencia SQLite e arquitetura em camadas.

## Visao geral
- Autenticacao e autorizacao com JWT e roles (Admin, User).
- CRUD de jogos e promocoes.
- Associacao de jogos a usuarios e a promocoes.
- Swagger habilitado em Development.
- Logs via NLog.

## Estrutura do repositorio
- FIAP.CloudGames.API: Web API (controllers, middleware, swagger, JWT).
- FIAP.CloudGames.Application: casos de uso e interfaces (repositorios, servicos).
- FIAP.CloudGames.Domain: entidades e regras de dominio.
- FIAP.CloudGames.Infrastructure: EF Core, repositorios, security, migrations.
- FIAP.CloudGames.Tests: testes unitarios dos casos de uso (xUnit, Moq).

## Tecnologias
- .NET 8 / ASP.NET Core
- Entity Framework Core + SQLite
- JWT Authentication
- Swagger / OpenAPI
- NLog
- xUnit + Moq

## Requisitos
- .NET SDK 8.x

## Como executar
```powershell
dotnet restore
dotnet run --project FIAP.CloudGames.API
```
Swagger: `http://localhost:5179/swagger`

## Banco de dados
- SQLite em arquivo: `FIAP.CloudGames.API/fiap.cloudgames.db`.
- Migrations em `FIAP.CloudGames.Infrastructure/Migrations`.

Para aplicar migrations:
```powershell
dotnet ef database update --project FIAP.CloudGames.Infrastructure --startup-project FIAP.CloudGames.API
```

## Autenticacao
1. `POST /api/auth/login` com email e senha.
2. Use o token retornado em `Authorization: Bearer {token}`.

## Endpoints principais
- Auth: `POST /api/auth/login`
- Games: `GET /api/games`, `GET /api/games/{id}`, `POST/PUT/DELETE /api/games/{id}` (Admin cria/edita/exclui; Admin e User leem).
- Promotions: `GET /api/promotions`, `GET /api/promotions/{id}`, `POST/PUT/DELETE /api/promotions/{id}`, `POST /api/promotions/{id}/games` (Admin cria/edita/exclui/associa).
- Users (Admin): `GET /api/users`, `GET /api/users/{id}`, `POST/PUT/DELETE /api/users/{id}`, `POST /api/users/{id}/games`

## Testes
```powershell
dotnet test
```

## Observacoes
- A chave JWT esta definida em `FIAP.CloudGames.API/Program.cs` e deve ser movida para configuracao segura em ambientes reais.
- 
- Como este e um projeto MVP, o primeiro usuario admin e criado na migration para facilitar o acesso inicial.
- Login admin: admin@admin.com
- Senha admin: Mudar@123
