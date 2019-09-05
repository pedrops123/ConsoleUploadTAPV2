using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Upload_GeralV1.Models
{

    [Table("CentroCusto", Schema = "Med")]
    public class CentroDeCusto
    {
        [Key]
        public int IdCentroCusto { get; set; } 
        public int IdEmpresa { get; set; }
        public int IdCliente { get; set; }
        public int  IdFilial { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string Tipo { get; set; }
        public bool Ativo { get; set; }

    }
}
