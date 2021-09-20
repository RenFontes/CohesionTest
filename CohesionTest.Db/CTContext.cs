using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CohesionTest.Db
{
    public class CTContext : DbContext
    {
        private readonly string dbPath;

        public virtual DbSet<ServiceRequest> ServiceRequests { get; set; }

        public CTContext() : this(null)
        {

        }

        public CTContext(string dbPath = null)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);

            // default path on Windows is C:\Users\{username}\AppData\Local\cohesion-test.db
            this.dbPath = dbPath ?? Path.Join(path, "cohesion-test.db");
            Console.WriteLine(this.dbPath);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // I would normally put the connection string in a .env file, but it's here now since I'm using sqlite for simplicity's sake.
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
    }
}
