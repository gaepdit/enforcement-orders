# Entity Framework database migrations

Instructions for adding a new Entity Framework database migration:

1. Open a command prompt to this folder ("./src/Infrastructure/").

2. Run the following command with an appropriate migration name:

    `dotnet ef migrations add NAME_OF_MIGRATION --msbuildprojectextensionspath ..\..\artifacts\Infrastructure\obj\`