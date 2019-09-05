using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Upload_GeralV1.ConectionBD
{
    public class DbContextFactory : IDesignTimeDbContextFactory<ContextConectionBd>
    {
        public static string _connectionString;

        public ContextConectionBd CreateDbContext()
        {
            return CreateDbContext(null);
        }

        public ContextConectionBd CreateDbContext(string[] args)
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                LoadConnectionString();
            }

            var builder = new DbContextOptionsBuilder<ContextConectionBd>();
            builder.UseSqlServer(_connectionString);

            return new ContextConectionBd(builder.Options);
        }


        private static void LoadConnectionString()
        {
            var builder = new ConfigurationBuilder();
            var PathProjeto = Directory.GetCurrentDirectory();
            var caminhoReplace = PathProjeto.Replace(@"\bin\Debug\netcoreapp2.2", "");
            builder.AddJsonFile(caminhoReplace + @"\appsettings.json", optional: false);

            var configuration = builder.Build();

            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public string RetornaConectionStrings()
        {
            return _connectionString;
        }


    }
}
