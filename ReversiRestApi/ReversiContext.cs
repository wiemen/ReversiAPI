using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ReversiRestApi.Models;
using ReversiRestApi.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace ReversiRestApi
{
    public class ReversiContext : DbContext
    {
        public ReversiContext(DbContextOptions<ReversiContext> options) : base(options)
        {
            Database.Migrate();
        }

        public DbSet<Spel> Spel { get; set; }
        public DbSet<Uitslag> Uitslag { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Spel>().Property(s => s.Bord).HasConversion(
                v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                v => JsonConvert.DeserializeObject<Kleur[,]>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })
            );
            builder.Entity<Spel>().Property(s => s.Beurten).HasConversion(
                v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                v => JsonConvert.DeserializeObject<List<ValueTuple<int, int, int>>>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })
            );
        }
    }
}
