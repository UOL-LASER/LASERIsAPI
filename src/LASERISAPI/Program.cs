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

app.MapGet("/entries", async (EntryDB db, int? id, string? name, string? manufacturerName, string? serialNumber, string? orderCode, string? itemType, int? quantity, string? signedOutTo, int? signedOutToId, DateTime? signedOutDate ) => {
    var query = db.Entries.AsQueryable();
    
    if (id.HasValue)
    {
        query = query.Where(entry => entry.id == id.Value);
    }

    if (!string.IsNullOrEmpty(name))
    {
        query = query.Where(entry => entry.name.Contains(name));
    }

    if (!string.IsNullOrEmpty(manufacturerName))
    {
        query = query.Where(entry => entry.manufacturerName.Contains(manufacturerName));
    }

    if (!string.IsNullOrEmpty(serialNumber))
    {
        query = query.Where(entry => entry.serialNumber.Contains(serialNumber));
    }

    if (!string.IsNullOrEmpty(orderCode))
    {
        query = query.Where(entry => entry.orderCode.Contains(orderCode));
    }

    if (!string.IsNullOrEmpty(itemType))
    {
        query = query.Where(entry => entry.itemType.Contains(itemType));
    }

    if(quantity.HasValue && (quantity.Value == 0 || quantity.Value == 1)) {
        query = query.Where(entry => entry.quantity == quantity.Value);
    }
    else if(quantity.HasValue) {
        query = query.Where(entry => entry.quantity > 1);
    }


    if (!string.IsNullOrEmpty(signedOutTo))
    {
        query = query.Where(entry => entry.signedOutTo.Contains(signedOutTo));
    }

    if (signedOutToId.HasValue)
    {
        query = query.Where(entry => entry.signedOutToId == signedOutToId.Value);
    }

    if (signedOutDate.HasValue)
    {
        query = query.Where(entry => entry.signedOutDate.HasValue && entry.signedOutDate.Value.Date == signedOutDate.Value.Date);
    }

    var results = await query.ToListAsync();
    return Results.Ok(results);

    
});




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
    findItem.productDescription = updateEntry.productDescription ?? findItem.productDescription;
    findItem.physicalDescription = updateEntry.physicalDescription ?? findItem.physicalDescription;
    findItem.productLink = updateEntry.productLink ?? findItem.productLink;
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
