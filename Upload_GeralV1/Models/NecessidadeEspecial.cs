using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Upload_GeralV1.Models
{
    [Table("TipoNecessidadeEspecial", Schema = "Med")]
    public class NecessidadeEspecial
    {
        [Key]
        public Int32 IdTipoNecessidadeEspecial { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }

    }
}
