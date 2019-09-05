using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Upload_GeralV1.Models
{
    [Table("Setor", Schema = "Med")]
    public class Setor
    {
        [Key]
        public int IdSetor { get; set; }
        public int? IdEmpresa { get; set; }
        public int?  IdCliente { get; set; }
        public int IdFilial { get; set; }
        public string Codigo { get; set; }
        public string CodigoRH { get; set; }   
        public string Nome { get; set; }
        public string  Descricao { get; set; }
        public string  ObservacaoASO { get; set; }
        public bool Ativo { get; set; }
        public Int16? NumeroOrdem { get; set; }

    }
}
