# Copilot Instructions – Argos (.NET)

These instructions apply to all code suggestions made by GitHub Copilot
for this repository.

## Project context

- Language: C#
- Framework: .NET 10
- Architecture: Clean Architecture
- Application type: CLI Tool
- Target OS: Cross-platform (Windows, Linux, macOS)


## Development environment

- Primary OS: Windows
- Shell: PowerShell
- All command-line examples must use PowerShell syntax
- Do not generate Bash, sh, zsh, or Linux-specific commands unless explicitly requested


## General principles

- Prefer clarity over cleverness
- Favor explicit code rather than magic or implicit behavior
- Follow SOLID principles
- Avoid unnecessary abstractions
- Use async/await end-to-end (no sync-over-async)

## C# / .NET conventions

- Nullable reference types are enabled
- Use `record` for DTOs and immutable data
- Use `sealed` for classes not intended to be inherited
- Prefer expression-bodied members when readable
- No `#region`
- Use `var` when the type is obvious from the right-hand side
- Use explicit types when it improves readability (e.g. `var` for primitives, but not for complex types)

## Naming

- PascalCase for public types and members
- camelCase for private fields and locals
- Do not use abbreviations unless well known (Id, Url, Http)
- One concept = one name (avoid synonyms)

## Architecture rules

- Domain layer has no dependency on EF Core or infrastructure
- Application layer contains use cases and orchestration
- Infrastructure layer contains EF Core, files, external services
- No direct reference from Domain/Application to Infrastructure

## Entity Framework Core

- Code First with explicit migrations
- No `EnsureCreated()`
- No `Database.Migrate()` at application startup (especially in prod)
- Prefer Fluent API over data annotations
- Stored procedures:
  - `FromSqlRaw` for single result sets
  - ADO.NET for multiple result sets

## Database modeling

- Prefer Value Objects over primitive obsession
- Avoid nullable columns unless semantically required
- Use UTC for all timestamps
- Avoid cascade delete unless explicitly intended

## Error handling

- No empty catch blocks
- Use domain-specific exceptions
- Do not swallow exceptions
- Log at boundaries, not everywhere

## Logging

- Use structured logging
- No string concatenation in logs
- No logging in tight loops

## Testing

- Test framework: xUnit
- Assertion library: FluentAssertions
- Naming: MethodName_ShouldExpectedBehavior_WhenCondition
- Unit tests must not touch the database
- EF Core testing uses SQLite or InMemory provider only

## What NOT to suggest

- Do not introduce new frameworks without justification
- Do not generate migrations automatically in runtime code
- Do not use static mutable state
- Do not use singleton services for business logic

## Style

- Keep methods under ~40 lines where possible
- One responsibility per method
- Prefer early returns over deep nesting