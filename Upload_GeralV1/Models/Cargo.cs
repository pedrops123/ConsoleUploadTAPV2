using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Upload_GeralV1.Models
{

    [Table("Cargo", Schema = "Med")]
    public class Cargo
    {
        [Key]
        public int IdCargo { get; set; }
        public int IdEmpresa { get; set; }
        public int IdCliente { get; set; }  
        public int IdFilial { get; set; }
        public string Codigo { get; set; }   
        public string CodigoRH { get; set; }
        public string CodigoCBO { get; set; }
        public string CodigoGFIP { get; set; }
        public string Nome { get; set; }
        public string NomeLegal { get; set; }
        public string Funcao { get; set; }
        public string RequisitoFuncao { get; set; }
        public string DescricaoDetalhada { get; set; }
        public string Educacao { get; set; }
        public string Treinamento { get; set; }
        public string Habilidade { get; set; }
        public string Experiencia { get; set; }
        public string DescricaoLocal { get; set; }
        public string OrientacaoASO { get; set; }
        public string MaterialUtilizado { get; set; }
        public string MobiliarioUtilizado { get; set; }
        public string LocalTrabalho { get; set; }
        public bool Ativo { get; set; }
        public Int16 NumeroOrdem { get; set; }
    }
}
