dotnet-ef migrations add InitialMigration --project Data.Context --startup-project Presentation
dotnet-ef database update --project Data\Data.Context --startup-project Presentation