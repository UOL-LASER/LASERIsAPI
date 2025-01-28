using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using LASERISAPI.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Entries") ?? "Data Source=Entries.db";

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSqlite<EntryDB>(connectionString);
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
app.MapGet("/entries/by-id/{id}", async (EntryDB db, int id) => await db.Entries.FindAsync(id));
app.MapGet("/entries/by-name/{name}", async (EntryDB db, String name) => await db.Entries.FindAsync(name));
app.MapGet("/entries/by-mname/{manufacturerName}", async (EntryDB db, String manufacturerName) => await db.Entries.FindAsync(manufacturerName));
app.MapGet("/entries/by-serialno/{serialNumber}", async (EntryDB db, String serialNumber) => await db.Entries.FindAsync(serialNumber));
app.MapGet("/entries/by-oCode/{orderCode}", async (EntryDB db, String orderCode) => await db.Entries.FindAsync(orderCode));
app.MapGet("/entries/by-type/{itemType}", async (EntryDB db, String itemType) => await db.Entries.FindAsync(itemType));

app.MapPost("/entry", async (EntryDB db, Entry newEntry) =>
{
    await db.Entries.AddAsync(newEntry);
    await db.SaveChangesAsync();
    return Results.Created($"/entry/{newEntry.id}", newEntry);
});

app.MapPut("/entry/{id}", async (EntryDB db, Entry updateEntry, int id) =>
{

    var findItem = await db.Entries.FindAsync(id);
    if (findItem is null) return Results.NotFound();
    findItem.name = updateEntry.name;
    findItem.manufacturerName = updateEntry.manufacturerName;
    findItem.description = updateEntry.description;
    findItem.serialNumber = updateEntry.serialNumber;
    findItem.orderCode = updateEntry.orderCode;
    findItem.itemType = updateEntry.itemType;
    findItem.quantity = updateEntry.quantity;

    await db.SaveChangesAsync();
    return Results.NoContent();

});

app.MapDelete("/entry/{id}", async (EntryDB db, int id) =>
{
   var finditem = await db.Entries.FindAsync(id);
   if (finditem is null) return Results.NotFound();

   db.Entries.Remove(finditem);
   await db.SaveChangesAsync();
   return Results.Ok();
});

app.Run();
