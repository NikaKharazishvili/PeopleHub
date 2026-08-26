# PeopleHub API
A people/contacts management REST API built with ASP.NET Core, Entity Framework Core, and SQL Server.
Demonstrates relational modeling (one-to-many, many-to-many), layered architecture, and public-read/owner-write authorization.

## Tech Stack
- ASP.NET Core Web API (.NET 9)
- Entity Framework Core (SQL Server)
- Swagger / Swashbuckle
- ASP.NET Identity + JWT Bearer Authentication

## Progress
### ✅ Stage 1 — Core CRUD API
- Models, DTOs (Create/Update/PartialUpdate/Read), and manual mapper (extension methods)
- Service layer (interface + implementation) separated from Controller
- Full async/await database access via EF Core
- RESTful Controller with proper status codes (200, 201, 204, 403, 404)
- Route constraints (`{id:int}`) for input safety and performance
- Relies on `[ApiController]`'s built-in model validation and binding inference
- Pagination on GetAll (`page`, `pageSize` query params, wrapped in `PagedResponse<T>`)
- Global exception handling middleware
- Structured logging via built-in `ILogger<T>` throughout the service layer
- Shared validation constants (`Constants.cs`) instead of magic numbers across DTOs
- **One-to-many**: `Person` → `Quote` (a person has many quotes)
- **Many-to-many**: `Person` ↔ `Interest` (people share a fixed, seeded Interest lookup list — never user-creatable via API)
- Seed data (`HasData`) for Interests, demo Persons, Quotes, and Person-Interest links, applied via migrations

### ✅ Stage 2 — Identity & Authorization
- ASP.NET Identity for user accounts (`User : IdentityUser`)
- JWT Bearer authentication — `AuthController` with Register/Login issuing tokens via `TokenService`
- Public read access (`GetAll`/`GetById` open to everyone, including anonymous users)
- Owner-only write access (`Create`/`Update`/`PartialUpdate`/`Delete` require the logged-in user to own the Person — returns `403 Forbidden` if not)
- 4 demo users auto-seeded on startup (matching the 4 demo Persons) — no manual setup needed to test ownership logic
- Swagger UI configured with Bearer token support (Authorize button)
- JWT signing key stored via `dotnet user-secrets`, not committed to source control