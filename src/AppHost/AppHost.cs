using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Enfo_WebApp>("WebApp");

await builder.Build().RunAsync();
