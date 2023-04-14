# RedShift

**Sample** .NET 7 application which utilizes **GraphQL** to interact with the system.
The main **goal** of this project is to explore capabilities of **GraphQL** and **HotChocolate** framework.
The structure is the following:
- GraphQL - provides an entry points to the application and data manipulation;
- Infrastructure - place where gatherd all implementation details, I/O operations, cross-cutting concerns and configurations;
- Application - the level responsible for orchestration and abstractions;
- Domain - business rules and models with POCO objects;
- UnitTests - project with unit tests;
- IntegrationTests - project with integration tests.

# Main Tools and Technologies

- HotChocolate
- MediatR
- Redis
- Postgres

# Requirements

- [.NET 7.0 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)
- [Docker](https://docs.docker.com/get-docker)

To start the required app's infrastructure via Docker, type the following command at the solution directory:

`docker compose up -d`