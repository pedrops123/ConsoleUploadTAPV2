using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Upload_GeralV1.Models
{
    [Table("TipoEndereco", Schema = "Adm")]
    public class TipoEndereco
    {
        [Key]
        public Int16 IdTipoEndereco { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
    }
}
