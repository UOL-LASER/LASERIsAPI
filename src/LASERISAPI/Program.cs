using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using LASERISAPI.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<EntryDB>(options => options.UseInMemoryDatabase("items"));
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

app.MapGet("/entries", async (EntryDB db) => await db.Entries.ToListAsync());

app.Run();
