using System;
using System.Collections.Generic;
using System.Text;
using Upload_GeralV1.Models;

namespace Upload_GeralV1.Utilities
{
    public class MontaClassesByObject
    {

        public KeyValuePair<bool, string> validaObjetoTransitoria(List<object> dadosLinhas, int numeroLinha)
        {
            string mensagemErroParse = "";
            KeyValuePair<bool, string> retornoValidacaoObject = new KeyValuePair<bool, string>();

            // Numero usado como base no teste ( Qualquer numero inteiro funciona aqui )
            int numeroTeste = 0;

            /* Verifica se codigo matriz esta como inteiro */

            bool testeCodigoMatriz = int.TryParse(dadosLinhas[1].ToString(), out numeroTeste);

            if (testeCodigoMatriz == false)
            {
                mensagemErroParse += " \n \n * Campo codigo Matriz na linha " + numeroLinha + " não esta como numerico. \n \n";
            }

            /* Verifica se codigo filial esta como inteiro */

            bool testeCodigoFilial = int.TryParse(dadosLinhas[4].ToString(), out numeroTeste);

            if (testeCodigoFilial == false)
            {
                mensagemErroParse += " \n \n * Campo codigo filial na linha " + numeroLinha + " não esta como numerico. \n \n";
            }

            /* Testa Data Admissao */
            if(dadosLinhas[17].ToString().Trim() != "" && dadosLinhas[17].ToString().Trim() != null)
            {
                try
                {
                    var TestedataAdmissao = DateTime.Parse(dadosLinhas[17].ToString());
                }
                catch
                {
                    mensagemErroParse += " \n \n * Campo Data Admissão na linha " + numeroLinha + " não esta no formato de data correta . \n \n";
                }
            }



            /* Testa Data Demissao */
            if (dadosLinhas[18].ToString().Trim() != "" && dadosLinhas[18].ToString().Trim() != null)
            {
                try
                {
                    var TestedataDemissao = DateTime.Parse(dadosLinhas[18].ToString());
                }
                catch
                {
                    mensagemErroParse += " \n \n * Campo Data Demissao na linha " + numeroLinha + " não esta no formato de data correta . \n \n";
                }
            }

            /* Testa Data Nascimento */
            if (dadosLinhas[19].ToString().Trim() != "" && dadosLinhas[19].ToString().Trim() != null)
            {
                try
                {
                    var TestedataAdmissao = DateTime.Parse(dadosLinhas[19].ToString());
                }
                catch
                {
                    mensagemErroParse += " \n \n * Campo Data Nascimento na linha " + numeroLinha + " não esta no formato de data correta . \n \n";
                }
            }
            /* Testa Data Emissao RG */
            if (dadosLinhas[25].ToString().Trim() != "" && dadosLinhas[25].ToString().Trim() != null)
            {
                try
                {
                    var TestedataEmissao = DateTime.Parse(dadosLinhas[25].ToString());
                }
                catch
                {
                    mensagemErroParse += " \n \n * Campo Emissao RG na linha " + numeroLinha + " não esta no formato de data correta . \n \n";
                }
            }

            /* Testa Ctps Data Emissao */
            if (dadosLinhas[29].ToString().Trim() != "" && dadosLinhas[29].ToString().Trim() != null)
            {
                try
                {
                    var TestedataCtps = DateTime.Parse(dadosLinhas[29].ToString());
                }
                catch
                {
                    mensagemErroParse += "\n \n * Campo CTPS Data Emissao na linha " + numeroLinha + " não esta no formato de data correta . \n \n";
                }
            }

            /* Validaçoes de Tamanho de campo */


            /* Retorno  validação */

            if(mensagemErroParse.Length > 0)
            {
                retornoValidacaoObject = new KeyValuePair<bool,string>(false,mensagemErroParse);
            }
            else
            {
                retornoValidacaoObject = new KeyValuePair<bool, string>(true,"");
            }

            return retornoValidacaoObject;

        }

        public ITFEmpregado montaObjetoTransitoria(List<object> colunasTabelas)
        {

            ITFEmpregado modeloDados = new ITFEmpregado();

            modeloDados.NumLinha = 0;
            try
            {
                modeloDados.CodMatriz = int.Parse(colunasTabelas[1].ToString());
            }
            catch
            {
                modeloDados.CodMatriz = null;
            }
            modeloDados.NomeMatriz = colunasTabelas[2].ToString().Replace("'", "");
            modeloDados.CnpjMatriz = colunasTabelas[3].ToString().Replace("'", "");

            try
            {
                modeloDados.CodFilial = int.Parse(colunasTabelas[4].ToString());
            }
            catch
            {
                modeloDados.CodFilial = null;
            }

            modeloDados.RazaoSocial = colunasTabelas[5].ToString().Replace("'", "");
            modeloDados.CnpjFilial = colunasTabelas[6].ToString().Replace("'", "");
            modeloDados.CodGHE = colunasTabelas[7].ToString().Replace("'", "");
            modeloDados.NomeGHE = colunasTabelas[8].ToString().Replace("'", "");
            modeloDados.CodCargo = colunasTabelas[9].ToString().Replace("'", "");
            modeloDados.NomeCargo = colunasTabelas[10].ToString().Replace("'", "");
            modeloDados.CodSetor = colunasTabelas[11].ToString().Replace("'", "");
            modeloDados.NomeSetor = colunasTabelas[12].ToString().Replace("'", "");
            modeloDados.CodCentroCusto = colunasTabelas[13].ToString().Replace("'", "");
            modeloDados.NomeCentroCusto = colunasTabelas[14].ToString().Replace("'", "");
            modeloDados.NumMatricula = colunasTabelas[15].ToString().Replace("'", "");
            modeloDados.NumChapa = colunasTabelas[16].ToString().Replace("'", "");

            try
            {
                modeloDados.DtAdmissao = DateTime.Parse(colunasTabelas[17].ToString());
            }
            catch
            {
                modeloDados.DtAdmissao = DateTime.MinValue;
            }

            try
            {
                modeloDados.DtDemissao = DateTime.Parse(colunasTabelas[18].ToString());
            }
            catch
            {
                modeloDados.DtDemissao = DateTime.MinValue;
            }

            try
            {
                modeloDados.DtNascimento = DateTime.Parse(colunasTabelas[19].ToString());
            }
            catch
            {
                modeloDados.DtNascimento = DateTime.MinValue;
            }     
            modeloDados.NomeFuncionario = colunasTabelas[20].ToString().Replace("'", "");
            modeloDados.CpfRne = colunasTabelas[21].ToString().Replace("'", "");
            modeloDados.RgUF = colunasTabelas[22].ToString().Replace("'", "");
            modeloDados.RgNumero = colunasTabelas[23].ToString().Replace("'", "");
            modeloDados.RgOrgaoEmissor = colunasTabelas[24].ToString().Replace("'", "");

            try
            {
                modeloDados.RgDtEmissao = DateTime.Parse(colunasTabelas[25].ToString());
            }
            catch
            {
                modeloDados.RgDtEmissao = DateTime.MinValue;
            }

            modeloDados.CtpsUf = colunasTabelas[26].ToString().Replace("'", "");
            modeloDados.CtpsNumero = colunasTabelas[27].ToString().Replace("'", "");
            modeloDados.CtpsSerie = colunasTabelas[28].ToString().Replace("'", "");

            try
            {
                modeloDados.CtpsDtEmissao = DateTime.Parse(colunasTabelas[29].ToString());
            }
            catch
            {
                modeloDados.CtpsDtEmissao = DateTime.MinValue;
            }

            modeloDados.EstadoCivil = colunasTabelas[30].ToString().Replace("'", "");
            modeloDados.Sexo = colunasTabelas[31].ToString().Replace("'", "");
            modeloDados.NumPisNit = colunasTabelas[32].ToString().Replace("'", "");
            modeloDados.NumGfip = colunasTabelas[33].ToString().Replace("'", "");
            modeloDados.TpContratacao = colunasTabelas[34].ToString().Replace("'", "");
            modeloDados.CategTrabalhador = colunasTabelas[35].ToString().Replace("'", "");
            modeloDados.NecessidadeEspecial = colunasTabelas[36].ToString().Replace("'", "");
            modeloDados.Situacao = colunasTabelas[37].ToString().Replace("'", "");
            modeloDados.DescrAtividade = colunasTabelas[38].ToString().Replace("'", "");
            modeloDados.NomeSupDireto = colunasTabelas[39].ToString().Replace("'", "");
            modeloDados.EmailSupDireto = colunasTabelas[40].ToString().Replace("'", "");
            modeloDados.AdicInsalubridade = colunasTabelas[41].ToString().Replace("'", "");
            modeloDados.GrauInsalubridade = colunasTabelas[42].ToString().Replace("'", "");
            modeloDados.AdicPericulosidade = colunasTabelas[43].ToString().Replace("'", "");
            modeloDados.GrauPericulosidade = colunasTabelas[44].ToString().Replace("'", "");
            modeloDados.NumCBO = colunasTabelas[45].ToString().Replace("'", "");
            modeloDados.DescrTurnoTrabalho = colunasTabelas[46].ToString().Replace("'", "");
            modeloDados.Lotacao = colunasTabelas[47].ToString().Replace("'", "");
            modeloDados.CEP = colunasTabelas[48].ToString().Replace("'", "");
            modeloDados.TpLogradouro = colunasTabelas[49].ToString().Replace("'", "");
            modeloDados.Logradouro = colunasTabelas[50].ToString().Replace("'", "");
            modeloDados.Numero = colunasTabelas[51].ToString().Replace("'", "");
            modeloDados.Complemento = colunasTabelas[52].ToString().Replace("'", "");
            modeloDados.Bairro = colunasTabelas[53].ToString().Replace("'","");
            modeloDados.UF = colunasTabelas[54].ToString().Replace("'", "");
            modeloDados.Cidade = colunasTabelas[55].ToString().Replace("'", "");
            modeloDados.Pais = colunasTabelas[56].ToString().Replace("'", "");
            modeloDados.Email = colunasTabelas[57].ToString().Replace("'", "");
            modeloDados.Telefone = colunasTabelas[58].ToString().Replace("'", "");
            modeloDados.Celular = colunasTabelas[59].ToString().Replace("'", "");
            modeloDados.NomeDependente = colunasTabelas[60].ToString().Replace("'", "");
            modeloDados.CpfDependente = colunasTabelas[61].ToString().Replace("'", "");
            modeloDados.TpDependente = colunasTabelas[62].ToString().Replace("'", "");
            try
            {
                modeloDados.MatriculaTaf = colunasTabelas[63].ToString();
            }
            catch
            {
                modeloDados.MatriculaTaf = "";
            }
            return modeloDados;
        }




    }
}
