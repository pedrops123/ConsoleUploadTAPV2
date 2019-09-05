using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Upload_GeralV1.Models
{


    [Table("Pessoa", Schema = "Adm")]
    public class Pessoa
    {
        [Key]
        public int IdPessoa { get; set; }
        public int IdEmpresa { get; set; }
        public int IdEvento { get; set; }
        public int IdAtividade { get; set; }
        public int IdTipoDocumento { get; set; }
        public int IdEnderecoPrincipal { get; set; }
        public int IdContatoPrincipal { get; set; }
        public int IdContaBancariaPrincipal { get; set; }
        public int IdImpostoPrincipal { get; set; }
        public int IdTipoClassificacaoPrincipal { get; set; }
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public string TipoPessoa { get; set; }
        public string CnpjCpf { get; set; }
        public string InscricaoEstadual { get; set; }
        public string InscricaoMunicipalSede { get; set; }
        public string InscricaoMunicipalLocal { get; set; }
        public string InscricaoInss { get; set; }
        public int IdOrgaoRegistroPessoa { get; set; }
        public string NumeroRegistro { get; set; }
        public string Observacao { get; set; }
        public int CodigoFuncionario { get; set; }
        public int CodigoExterno { get; set; }
        public bool CentroCustoFixo { get; set; }
        public bool OptanteSimples { get; set; }
        public bool ReterIss { get; set; }
        public int IdUsuarioInclusao { get; set; }
        public DateTime DataUsuarioInclusao { get; set; }
        public int IdUsuarioAlteracao { get; set; }
        public DateTime DataUsuarioAlteracao { get; set; }
        public bool Ativo { get; set; }
    }
}
