using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Upload_GeralV1.Models
{
    [Table("EnderecoEmpregado", Schema = "Med")]
    public class EnderecoEmpregado
    {
        [Key]
        public Int64 IdEnderecoEmpregado { get; set; }
        public int IdEmpresa { get; set; }
        public Int64 IdEmpregado { get; set; }
        public Int32? IdTipoEndereco { get; set; }
        public Int32? IdTipoLogradouro { get; set; }
        public string TipoLogradouro { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
        public string Pais { get; set; }
        public string Cep { get; set; }
        public string Observacao { get; set; }
        public bool Principal { get; set; }
        public int IdUsuarioInclusao { get; set; }

  
        public DateTime? DataUsuarioInclusao { get; set; }
        public int IdUsuarioAlteracao { get; set; }


        public DateTime? DataUsuarioAlteracao { get; set; }

    }
}
