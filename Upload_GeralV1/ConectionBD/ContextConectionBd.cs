using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Upload_GeralV1.Models;

namespace Upload_GeralV1
{
    public class ContextConectionBd : DbContext
    {

        public ContextConectionBd(DbContextOptions<ContextConectionBd> options) : base(options)
        {

        }

        #region tabelas_Sistemas

        public DbSet<ITFEmpregado> TransitoriaEmpregados { get; set; }
        public DbSet<Setor> TabelaSetor { get; set; }
        public DbSet<CentroDeCusto> TabelaCentroCusto { get; set; }
        public DbSet<Cargo> TabelaCargo { get; set; }
        public DbSet<Empregados> TabelaEmpregados { get; set; }
        public DbSet<EstadoCivil> TabelaEstadoCivil { get; set; }
        public DbSet<CategoriaTrabalhador> TabelaCategoriaTrabalhador { get; set; }
        public DbSet<TipoContratacao> TabelaTipoContratacao { get; set; }
        public DbSet<SituacaoEmpregado> TabelaSituacaoEmpregado { get; set; }
        public DbSet<NecessidadeEspecial> TabelaNecessidadeEspecial { get; set; }
        public DbSet<EnderecoEmpregado> TabelaEnderecoEmpregado { get; set; }
        public DbSet<Dependente> TabelaDependente { get; set; }
        public DbSet<Pessoa> TabelaPessoa { get; set; }
        public DbSet<TipoEndereco> TabelaTpEndereco { get; set; }
        public DbSet<TipoLogradouro> TabelaTpLogradouro { get; set; }
        public DbSet<TipoDependente> TabelaTipoDependente { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Empregados 

            modelBuilder.Entity<Empregados>()
                .Property(p => p.IdTipoContratacao)
                .HasColumnType("SMALLINT");

            modelBuilder.Entity<Empregados>()
                .Property(p => p.IdSituacaoEmpregado)
                .HasColumnType("SMALLINT");

            modelBuilder.Entity<Empregados>()
                .Property(p => p.IdTipoNecessidadeEspecial)
                .HasColumnType("SMALLINT");

            // Necessidade especial

            modelBuilder.Entity<NecessidadeEspecial>()
                .Property(p => p.IdTipoNecessidadeEspecial)
                .HasColumnType("SMALLINT");

            // Endereco empregado

            modelBuilder.Entity<EnderecoEmpregado>()
                .Property(p => p.IdTipoEndereco)
                .HasColumnType("SMALLINT");

            modelBuilder.Entity<EnderecoEmpregado>()
                .Property(p => p.IdTipoLogradouro)
                .HasColumnType("SMALLINT");

            modelBuilder.Entity<EnderecoEmpregado>()
                .Property(p => p.DataUsuarioAlteracao)
                .HasColumnType("smalldatetime");

            modelBuilder.Entity<EnderecoEmpregado>()
                .Property(p => p.DataUsuarioInclusao)
                .HasColumnType("smalldatetime");

            // Tabela Dependentes

            modelBuilder.Entity<Dependente>()
                .Property(f => f.DataAlteracao)
                .HasColumnType("smalldatetime");

            modelBuilder.Entity<Dependente>()
              .Property(f => f.DataInclusao)
              .HasColumnType("smalldatetime");

        }

    }
}
