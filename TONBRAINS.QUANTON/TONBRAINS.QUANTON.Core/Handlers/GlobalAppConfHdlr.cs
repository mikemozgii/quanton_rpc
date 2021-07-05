using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using TONBRAINS.QUANTON.Core.DAL;

namespace TONBRAINS.QUANTON.Core.Handlers
{
    public static class GlobalAppConfHdlr
    {
        public static string ConfiPath { get; set; } = "C:\\Configs\\qunaton_matrix_app.json";
        public static string QunatonDbConnectionString { get; set; }
        public static string AppVersion { get; set; }
       
        public static DbContextOptions GetQunatonDbContextOption()
        {
     
            var builder = new DbContextOptionsBuilder<QuantonDbContext>().UseNpgsql(QunatonDbConnectionString);
            return builder.Options;
        }


        private static IConfiguration configuration;
        static GlobalAppConfHdlr()
        {
            var builder = new ConfigurationBuilder().AddJsonFile(ConfiPath, optional: true, reloadOnChange: true);
            configuration = builder.Build();

         QunatonDbConnectionString = Get(nameof(QunatonDbConnectionString));
         AppVersion = Get(nameof(AppVersion));
        }

        public static string Get(string name)
        {
            return configuration[name];
        }
    }
}
