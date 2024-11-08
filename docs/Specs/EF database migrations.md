# Entity Framework database migrations

To create a new Entity Framework database migration, first install or update the Entity Framework tools:

`dotnet tool install --global dotnet-ef`

`dotnet tool update --global dotnet-ef`

Then from the root directory of the solution, run the following (replace "MigrationName"):

`dotnet ef migrations add MigrationName -s src\Infrastructure --msbuildprojectextensionspath .artifacts\Infrastructure\obj\`
