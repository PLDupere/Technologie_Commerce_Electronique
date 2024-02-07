using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics.Metrics;
using System;

namespace Boutique_en_ligne
{
    public class BoutiqueJeuDbContext : DbContext
    {
        public DbSet<Models.CarteCredit> CarteCredits { get; set; }
        public DbSet<Models.Client> Clients { get; set; }
        public DbSet<Models.Facture> Factures { get; set; }
        public DbSet<Models.JeuVideo> JeuVideos { get; set; }
        public DbSet<Models.Utilisateur> Utilisateurs { get; set; }
        public DbSet<Models.Vendeur> Vendeurs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            string connection_string = "Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = master; Integrated Security = True; Connect Timeout = 30; Encrypt = False; Trust Server Certificate = False; Application Intent = ReadWrite; Multi Subnet Failover = False";
            string database_name = "BoutiqueJeuDB";
            dbContextOptionsBuilder.UseSqlServer($"{connection_string};Database={database_name};");
        }
    }
}
