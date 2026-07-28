# TC1.RepairShop

Back-end (MVP) for an auto repair shop management system — Tech Challenge (SOAT).
This first step delivers only the project bootstrap: solution structure, initial
domain entities, database schema, JWT authentication and one protected sample
endpoint. Full CRUDs, automatic quoting, approval workflow, and inventory
management arrive in later prompts, one per bounded context.

## Domain overview

The domain was modeled via Event Storming into these bounded contexts:

- **Registration** (generic subdomain) — `Customer` and `Vehicle` aggregates.
- **ServiceOrders** (core domain) — `ServiceOrder` aggregate root, references
  `Customer` and `Vehicle` only by Id.
- **Quotes** (core domain) — `Quote` aggregate, tracks its own rejection
  history (the "rejection limit" rule).
- **Parts** / inventory (supporting subdomain) — `Part` and `PartRequest`
  aggregates (the latter arrives in a future prompt).

## Architecture

Modular monolith, organized internally by bounded context so it can be split
into services later if needed:

```
TC1.RepairShop.sln
src/
  TC1.RepairShop.Domain/          # POCOs, value objects, no framework dependencies
  TC1.RepairShop.Application/     # Use cases, ports (interfaces), JWT token service
  TC1.RepairShop.Infrastructure/  # Dapper repositories (data access at runtime)
  TC1.RepairShop.Migrations/      # Standalone FluentMigrator console app (schema + seed)
  TC1.RepairShop.Api/             # ASP.NET Core Web API, controllers, JWT wiring
tests/
  TC1.RepairShop.UnitTests/
  TC1.RepairShop.IntegrationTests/
docker/
  Dockerfile               # API image
  Dockerfile.migrations    # Migrations runner image
  docker-compose.yml
```

## Stack

- .NET 10, ASP.NET Core Web API.
- **Dapper only at runtime — no EF Core, no other ORM.** Data access via
  `Microsoft.Data.SqlClient` + raw Dapper in `TC1.RepairShop.Infrastructure`.
- SQL Server (Docker). Schema is versioned and applied by a **separate
  console app**, `TC1.RepairShop.Migrations`, using
  [FluentMigrator](https://fluentmigrator.github.io/) — the same pattern used
  in this team's other services (e.g. `Nexp.Infra.Database`): one C# class
  per migration under `Migrations/`, named `<timestamp>_<Description>.cs` and
  decorated with `[Migration(<timestamp>)]`. FluentMigrator tracks applied
  migrations in its own `VersionInfo` table and supports `--down <timestamp>`
  for rollbacks. The runner also creates the database if missing and seeds
  the `admin` user. It runs **before** the API starts (as its own step/container),
  never inside the API process.
- JWT authentication (`Microsoft.AspNetCore.Authentication.JwtBearer`) for
  administrative routes.
- Swagger/OpenAPI via Swashbuckle.
- Docker + docker-compose (migrations job + API + SQL Server).
- xUnit for tests.

### Why Dapper (not EF Core) for data access, and FluentMigrator (not EF Migrations) for schema

Runtime data access stays on raw Dapper + `Microsoft.Data.SqlClient` — no
change-tracking, no LINQ-to-SQL translation to reason about, full control
over generated SQL for a small, explicit domain like this one.

Schema evolution uses FluentMigrator rather than hand-written `.sql` scripts
or EF Core Migrations, to match the migration approach already used
elsewhere in this codebase (`Nexp.Infra.Database`): versioned, timestamped
C# migration classes, run by a dedicated console app/container instead of
being embedded in the API's startup path. This keeps "evolve the schema" and
"serve requests" as two independently deployable concerns, and keeps the
approach consistent with the team's other services.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (for the
  containerized run)
- SQL Server 2022 (only if running without Docker)

## Running with Docker (recommended)

From the repository root:

```bash
docker compose -f docker/docker-compose.yml up --build
```

This starts SQL Server, runs the `migrations` job (creates the database,
applies all FluentMigrator migrations, seeds the `admin` user), and only then
starts the API — `api` depends on `migrations` completing successfully
(`condition: service_completed_successfully`).

- API: http://localhost:8080/swagger
- SQL Server: `localhost,1433` (sa / `Passw0rd!Dev` by default)

Override defaults with environment variables before running compose:

| Variable              | Default                                    | Purpose                          |
|-----------------------|---------------------------------------------|-----------------------------------|
| `SA_PASSWORD`          | `Passw0rd!Dev`                              | SQL Server `sa` password          |
| `JWT_SECRET`           | `change-this-secret-in-production-min-32-chars` | JWT signing key              |
| `SEED_ADMIN_PASSWORD`  | `Admin@123`                                 | Password for the seeded `admin` user |

## Running without Docker

1. Start a local SQL Server 2022 instance.
2. Apply the schema and seed the admin user by running the migrations
   project (configure `ConnectionStrings:Default` and `SeedAdmin:Password` in
   `src/TC1.RepairShop.Migrations/appsettings.Development.json`, or via
   environment variables — `ConnectionStrings__Default`,
   `SeedAdmin__Password`/`SEED_ADMIN_PASSWORD`):

   ```bash
   dotnet run --project src/TC1.RepairShop.Migrations
   ```

   This creates the database if it doesn't exist, applies every pending
   migration, and seeds the `admin` user. It's idempotent — safe to run again
   (already-applied migrations and an existing seed user are skipped). To
   roll back, pass `--down <timestamp>` (see `dotnet run --project
   src/TC1.RepairShop.Migrations -- --help`).
3. Set the API's connection string and JWT secrets, either in
   `src/TC1.RepairShop.Api/appsettings.Development.json` or via environment
   variables / `dotnet user-secrets`:
   - `ConnectionStrings:DefaultConnection`
   - `Jwt:Secret`, `Jwt:Issuer`, `Jwt:Audience`
4. Run the API:

   ```bash
   dotnet run --project src/TC1.RepairShop.Api
   ```

## Testing the login flow

```bash
curl -X POST http://localhost:8080/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"Admin@123"}'
```

Use the returned token as a `Bearer` token against the sample protected
endpoint:

```bash
curl http://localhost:8080/api/customers \
  -H "Authorization: Bearer <token>"
```

- No token → `401 Unauthorized`
- Valid token → `200 OK` (empty list for now — `CustomersController` is a
  stub until the Registration bounded context prompt)

## Running the tests

```bash
dotnet test
```

- **`TC1.RepairShop.UnitTests`**: CPF/CNPJ (`NationalId`) validation, license
  plate (`LicensePlate`) validation (legacy and Mercosul formats), and JWT
  token generation/claims.
- **`TC1.RepairShop.IntegrationTests`**: exercises `POST /api/auth/login` and
  the protected `GET /api/customers` endpoint end-to-end through
  `WebApplicationFactory<Program>`. To keep `dotnet test` runnable without a
  live database or containers, this project swaps in an in-memory fake
  `IUserRepository` (seeded with the same `admin` / `Admin@123` credentials)
  — no SQL Server instance is required to run these tests.

This bootstrap step was also verified once against a real local SQL Server
instance (schema creation, idempotent re-run, seed, login, and the protected
endpoint's 401/200 behavior) outside of the automated test suite.

## Assumptions made in this bootstrap step

- All identifiers (classes, namespaces, SQL tables/columns, API routes) are in
  English; only the bounded context names in this document keep their
  conceptual meaning from the original Event Storming.
- The seed `admin` user's password comes from configuration
  (`SeedAdmin:Password`) or the `SEED_ADMIN_PASSWORD` environment variable —
  never hardcoded — and is hashed with BCrypt before being stored.
- Repository interfaces for `Customer`, `Vehicle`, `ServiceOrder`, `Quote` and
  `Part` are stubbed (no implementation) in `Infrastructure/Repositories`;
  only `IUserRepository`/`UserRepository` are implemented, since login is the
  only flow in scope for this step.
- `docker-compose.yml` lives under `docker/` (per the requested layout) with
  its build `context` pointing at the repository root; run it with
  `docker compose -f docker/docker-compose.yml up`.
- `TC1.RepairShop.Migrations` pulls in FluentMigrator 3.3.2, whose transitive
  `System.Drawing.Common` 4.7.0 dependency has a known advisory
  (GHSA-rxg9-xrhp-64gj). This is the same FluentMigrator version already used
  by this team's other services; no newer FluentMigrator release resolves it
  at time of writing. It only affects the offline migrations tool, not the
  API's runtime dependency graph.

## Out of scope for this step

- Full CRUD for any entity beyond what login requires.
- Automatic quote generation, approval/rejection workflow.
- Inventory management, low-stock alerts, mechanic notifications.
- `ServiceOrder` state machine rules (next prompt).
