using Microsoft.EntityFrameworkCore;
using System;
namespace LASERISAPI.Models 
{
    public class Entry
    {
        public int id { get; set; }
        public string name { get; set; }
        public string? manufacturerName { get; set; }
        public string? description { get; set; }
        public string? serialNumber { get; set; }
        public string? orderCode { get; set; }
        public string itemType { get; set; }
        public int quantity { get; set; }
        public string? signedOutTo { get; set; }
        public int? signedOutToId { get; set; }
        public DateTime? signedOutDate { get; set; }

    }

    class EntryDB : DbContext
    {
        public EntryDB(DbContextOptions options) : base(options) {}
        public DbSet<Entry> Entries { get; set; } = null!;
    }
}