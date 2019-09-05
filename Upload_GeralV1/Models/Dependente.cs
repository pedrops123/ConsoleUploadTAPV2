using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Upload_GeralV1.Models
{

    [Table("Dependente", Schema = "Med")]
    public class Dependente
    {
        [Key]
        public Int64 IdDependente { get; set; }
        public Int64 IdEmpregado { get; set; }
        public int IdEmpresa { get; set; }
        public Int64 IdItemAditivo { get; set; }
        public int? IdTipoDependente { get; set; }
        public string Nome { get; set; }
        public string CnpjCpf { get; set; }
        public int? IdGrauParentesco { get; set; }
        public decimal PercentualParticipacao { get; set; }
        public decimal ValorParticipacao { get; set; }
        public int IdFormaPagamento { get; set; }
        public string Banco { get; set; }
        public string Agencia { get; set; }
        public string AgenciaDigito { get; set; }
        public string Conta { get; set; }
        public string ContaDigito { get; set; }
        public int TipoConta { get; set; }
        public int? IdUsuarioInclusao { get; set; }
        public DateTime? DataInclusao { get; set; }
        public int? IdUsuarioAlteracao { get; set; }
        public DateTime? DataAlteracao { get; set; }

    }
}
