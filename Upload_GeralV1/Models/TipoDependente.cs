using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Upload_GeralV1.Models
{
    [Table("TipoDependente", Schema = "Med")]
    public class TipoDependente
    {
        [Key]
        public int IdTipoDependente { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }

    }
}
