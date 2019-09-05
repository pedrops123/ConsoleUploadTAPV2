using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using System.Text;

namespace Upload_GeralV1.Models
{

    [Table("ITF_Empregado", Schema = "Med")]
    public class ITFEmpregado
    {
       [Key]
       public int ID_ITF { get; set; }
       public string  CdChaveAcao { get; set; }
       public DateTime? DtChaveAcao { get; set; }
       public int NumLinha { get; set; }
       public int? CodMatriz { get; set; }
       public string  NomeMatriz { get; set; }
       public string CnpjMatriz { get; set; }
       public int?   CodFilial { get; set; }
       public string RazaoSocial { get; set; }
       public string CnpjFilial { get; set; }
       public string  CodGHE { get; set; }
       public string NomeGHE { get; set; }
       public string CodCargo { get; set; }
       public string NomeCargo { get; set; }
       public string CodSetor { get; set; }
       public string NomeSetor { get; set; }
       public string CodCentroCusto { get; set; }
       public string NomeCentroCusto { get; set; }
       public string NumMatricula { get; set; }
       public string  NumChapa { get; set; }
       public DateTime? DtAdmissao { get; set; }
       public DateTime? DtDemissao { get; set; }
       public DateTime? DtNascimento { get; set; }
       public string  NomeFuncionario { get; set; }
       public string CpfRne { get; set; }
       public string  RgUF { get; set; }
       public string RgNumero { get; set; }
       public string RgOrgaoEmissor { get; set; }
       public DateTime? RgDtEmissao { get; set; }
       public string CtpsUf { get; set; }
       public string CtpsNumero { get; set; }
       public string CtpsSerie { get; set; }
       public DateTime? CtpsDtEmissao { get; set; }
        public string EstadoCivil { get; set; }
        public string Sexo { get; set; }
        public string NumPisNit { get; set; }
        public string NumGfip { get; set; }
        public string TpContratacao { get; set; }
        public string CategTrabalhador { get; set; }
        public string NecessidadeEspecial { get; set; }
        public string Situacao { get; set; }
        public string DescrAtividade { get; set; }
        public string NomeSupDireto { get; set; }
        public string EmailSupDireto { get; set; }
        public string AdicInsalubridade { get; set; }
        public string GrauInsalubridade { get; set; }
        public string AdicPericulosidade { get; set; }
        public string GrauPericulosidade { get; set; }
        public string NumCBO { get; set; }
        public string DescrTurnoTrabalho { get; set; }
        public string Lotacao { get; set; }
        public string CEP { get; set; }
        public string TpLogradouro { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string UF { get; set; }
        public string Cidade { get; set; }
        public string Pais { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }
        public string NomeDependente { get; set; }
        public string CpfDependente { get; set; }
        public string TpDependente { get; set; }
        public string MsgCritica { get; set; }
        public DateTime? DtCritica { get; set; }
        public int CtrlCritica { get; set; }
        public int? IdEmpregado { get; set; }
        [NotMapped]
        public string MatriculaTaf { get; set; }
    }
}
