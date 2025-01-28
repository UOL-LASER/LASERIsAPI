using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "LASERISAPI", Description = "Lab Inventory System", Version = "v1" });
    });

}

var app = builder.Build();

if (builder.Environment.IsDevelopment()) {
 app.UseSwagger();
   app.UseSwaggerUI(c =>
    {
      c.SwaggerEndpoint("/swagger/v1/swagger.json", "LASERISAPI V1");
    });
} // end of if (app.Environment.IsDevelopment()) block

app.MapGet("/", () => "Hello World!");

app.Run();
