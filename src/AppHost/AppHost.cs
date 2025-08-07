using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<WebApp>(nameof(WebApp));

await builder.Build().RunAsync();
