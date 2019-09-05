using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Upload_GeralV1.Models
{

    [Table("SituacaoEmpregado", Schema = "Med")]
    public class SituacaoEmpregado
    {
        [Key]
        public Int16 IdSituacaoEmpregado { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }
    }
}
