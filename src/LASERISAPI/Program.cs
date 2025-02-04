using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using LASERISAPI.Models;
using System;

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
app.MapGet("/entries/by-name/{name}", async (EntryDB db, String name) => await db.Entries.Where(e => e.name.ToLower() == name.ToLower()).ToListAsync());
app.MapGet("/entries/by-mname/{manufacturerName}", async (EntryDB db, String manufacturerName) => await db.Entries.Where(e => e.manufacturerName != null && e.manufacturerName.ToLower() == manufacturerName.ToLower()).ToListAsync());
app.MapGet("/entries/by-serialno/{serialNumber}", async (EntryDB db, String serialNumber) => await db.Entries.Where(e => e.serialNumber != null && e.serialNumber.ToLower() == serialNumber.ToLower()).ToListAsync());
app.MapGet("/entries/by-oCode/{orderCode}", async (EntryDB db, String orderCode) => await db.Entries.Where(e => e.orderCode != null && e.orderCode.ToLower() == orderCode.ToLower()).ToListAsync());
app.MapGet("/entries/by-type/{itemType}", async (EntryDB db, String itemType) => await db.Entries.Where(e => e.itemType.ToLower() == itemType.ToLower()).ToListAsync());
app.MapGet("/entries/by-quantity/{quantity}", async (EntryDB db, int quantity) => await db.Entries.Where(e => e.quantity == quantity).ToListAsync());
app.MapGet("/entries/by-signedoutto/{signedOutTo}", async (EntryDB db, String signedOutTo) => await db.Entries.Where(e => e.signedOutTo != null && e.signedOutTo.ToLower() == signedOutTo.ToLower()).ToListAsync());
app.MapGet("/entries/by-signedouttoid/{signedOutToId}", async (EntryDB db, int signedOutToId) => await db.Entries.Where(e => e.signedOutToId != null && e.signedOutToId == signedOutToId).ToListAsync());
app.MapGet("/entries/by-signedoutdate/{signedOutDate}", async (EntryDB db, DateTime signedOutDate) => await db.Entries.Where(e => e.signedOutDate != null && e.signedOutDate.Value.Date == signedOutDate.Date).ToListAsync());





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
    findItem.name = updateEntry.name ?? findItem.name;
    findItem.manufacturerName = updateEntry.manufacturerName ?? findItem.manufacturerName;
    findItem.description = updateEntry.description ?? findItem.description;
    findItem.serialNumber = updateEntry.serialNumber ?? findItem.serialNumber;
    findItem.orderCode = updateEntry.orderCode ?? findItem.orderCode;
    findItem.itemType = updateEntry.itemType ?? findItem.itemType;
    findItem.quantity = updateEntry.quantity > 0 ? updateEntry.quantity : findItem.quantity;
    findItem.signedOutTo = updateEntry.signedOutTo ?? findItem.signedOutTo;
    findItem.signedOutToId = updateEntry.signedOutToId ?? findItem.signedOutToId;
    findItem.signedOutDate = updateEntry.signedOutDate ?? findItem.signedOutDate;

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
