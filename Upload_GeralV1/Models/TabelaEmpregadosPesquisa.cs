using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Upload_GeralV1.Models
{
    [Table("Empregado", Schema = "Med")]
    public class TabelaEmpregadosPesquisa
    {
        [Key]
        public Int64 IdEmpregado { get; set; }
        public int IdEmpresa { get; set; }
        public int IdCliente { get; set; }
        public int IdFilial { get; set; }
        public int? IdCargo { get; set; }
        public int? IdSetor { get; set; }
        public int? IdCentroCusto { get; set; }
        public Int64? IdEnderecoPrincipal { get; set; }
        public int? IdContatoPrincipal { get; set; }
        public int? IdContaPrincipal { get; set; }
        public int? IdCategoriaTrabalhador { get; set; }
        public Int32? IdTipoContratacao { get; set; }
        public Int32? IdSituacaoEmpregado { get; set; }
        public Int32? IdTipoNecessidadeEspecial { get; set; }
        public string NumeroChapa { get; set; }
        public string Matricula { get; set; }
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Sexo { get; set; }
        public string TipoPessoa { get; set; }
        public string CnpjCpf { get; set; }
        public string RgUF { get; set; }
        public string Rg { get; set; }
        public string RgOrgao { get; set; }
        public string RgDataEmissao { get; set; }
        public string CtpsUF { get; set; }
        public string Ctps { get; set; }
        public string CtpsSerie { get; set; }
        public string CtpsDataEmissao { get; set; }
        public string Pis { get; set; }
        public string Gfip { get; set; }
        public string Observacao { get; set; }
        public string DataAdmissao { get; set; }
        public string DataDemissao { get; set; }
        public bool Periculosidade { get; set; }
        public bool Contato { get; set; }
        public bool Ativo { get; set; }
        public int IdUsuarioInclusao { get; set; }
        public DateTime DataUsuarioInclusao { get; set; }
        public int? IdUsuarioAlteracao { get; set; }
        public DateTime DataUsuarioAlteracao { get; set; }
        public string Telefone { get; set; }
        public int? IdEstadoCivil { get; set; }
        public bool Insalubridade { get; set; }
        public string GrauInsalubridade { get; set; }
        public string GrauPericulosidade { get; set; }
        public string TurnoTrabalho { get; set; }
        public string MatriculaESocial { get; set; }

    }
}
