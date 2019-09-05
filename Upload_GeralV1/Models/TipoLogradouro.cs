using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Upload_GeralV1.Models
{
    [Table("TipoLogradouro", Schema = "Adm")]
    public class TipoLogradouro
    {
        [Key]
        public Int16 IdTipoLogradouro { get; set; }
        public string Abreviado { get; set; }
        public string Nome { get; set; }
        public string Codigo { get; set; }
    }
}
