<!-- d7d946ab-3f5c-4afa-b11f-283e0b0d2184 c108b5e7-acb2-4ef5-b436-552ba9145e79 -->
# New Service Blueprint (Clean Architecture + CQRS + Mediator)

## Solution Layout
- src/
  - <Product>.Web (ASP.NET Core Web API; controllers only)
  - <Product>.Contract (CQRS contracts, DTOs)
  - <Product>.Core (Handlers, domain services, interfaces, mappings)
  - <Product>.Domain (Entities, value objects)
  - <Product>.Infrastructure (EF Core, repositories, Kafka, cache, storage)
- tests/
  - <Product>.Test (unit/integration tests)
- docker-compose.yml (db, redis, minio, kafka)

## Packages
- Web: Swashbuckle.AspNetCore, Asp.Versioning.Mvc.ApiExplorer, MediatR
- Core: MediatR
- Infrastructure: EF Core + Npgsql, EF Tools/Design, StackExchangeRedis, FusionCache, Minio, Confluent.Kafka

## Cross-cutting Contracts (Contract project)
- Message: `ICommand<T>`, `IQuery<T>`, `ICommandHandler<TCmd,T>`, `IQueryHandler<TQry,T>`
- Shared: `BaseResponseDto<T>`; paging primitives (optional)
- TransferObjects: DTOs per aggregate
- UseCases per bounded context (e.g., Notification, Email, Storage):
  - `Command.cs` (records), `Query.cs` (records), `Request.cs`, `Response.cs`

## Core (Application)
- Interfaces: repositories, unit of work, cache, storage, email, kafka, configuration
- Handlers per UseCase (Command/Query)
- Mapping extensions Entity → DTO
- Validation inside handlers (early returns, consistent BaseResponseDto)

## Domain
- Entities inherit `BaseEntity { Guid Id }`
- Aggregates kept persistence-agnostic

## Infrastructure
- EF `AppDataContext` with DbSets and Fluent configs
- Generic repository `IGenericRepository<T>` + `EfUnitOfWork`
- Cache: FusionCache + optional Redis distributed cache
- Kafka: `IKafkaProducer`, `IKafkaRepository<T>`, hosted consumer `KafkaConsumerHostedService<T>`
- Storage: MinIO client wiring and `IStorageService`
- Email: SMTP/IMAP service if needed

## Web (Presentation)
- Controllers are thin; inject `ISender` and send Commands/Queries
- Versioning `[ApiVersion(1)]`, Swagger doc, consistent `BaseResponseDto<T>` responses

## App Startup (Program.cs)
- AddControllers, Swagger, ApiVersioning
- Register interfaces → implementations (Core/Infrastructure)
- Add DbContext (connection string)
- Add FusionCache (+ Redis if configured)
- Add MediatR: `RegisterServicesFromAssembly(Core, Contract)`
- Hosted services: Kafka consumers

## Example Feature (Template + UserCv)
- Contract: DTOs, Requests, Commands/Queries for CRUD
- Core: Handlers using `IGenericRepository<>`, transactions, `Include(...)` for navigation
- Infrastructure: DbSets + Fluent API (relations, soft delete flags)
- Web: `CvTemplatesController`, `UserCvsController` with CRUD endpoints

## Observability & Quality
- Logging via built-in ILogger
- Unit tests for handlers (mock repositories)
- Optional integration tests with Testcontainers (Postgres/Redis/MinIO/Kafka)

## Deployment
- Dockerfile for Web; docker-compose with Postgres, Redis, MinIO, Kafka
- Environment via appsettings + secrets

## Notes & Conventions
- Records for Commands/Queries; DTOs are classes
- Use UTC times; soft-delete via flags
- Include related data using EF `.Include(...)` via repository include expression
- Keep controllers free of business logic


### To-dos

- [ ] Create solution and 5 projects with folder structure
- [ ] Add NuGet packages to each project per blueprint
- [ ] Create Message, Shared, DTOs, UseCases in Contract
- [ ] Add BaseEntity and initial aggregates
- [ ] Add Core interfaces (repos, services, cache, storage, kafka)
- [ ] Implement EF, repositories, UoW, cache, storage, kafka
- [ ] Configure DI, DbContext, FusionCache, MediatR, Swagger, hosted services
- [ ] Implement Template and UserCv CRUD end-to-end
- [ ] Add versioned controllers using MediatR ISender
- [ ] Add unit tests for handlers and repository abstractions
- [ ] Dockerfile and docker-compose for db/redis/minio/kafka