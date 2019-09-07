using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upload_GeralV1.ConectionBD;
using Upload_GeralV1.Models;
using Upload_GeralV1.Utilities;

namespace Upload_GeralV1
{
    public class CargasGeralClass
    {
        DbContextFactory contexto;
        MontaClassesByObject contextoMontaObject;

        //Abre Construtor 
        public CargasGeralClass()
        {
            // Inicializa contexto
            contexto = new DbContextFactory();
            contextoMontaObject = new MontaClassesByObject();
        }

        public KeyValuePair<bool, string> testaConexaoBD()
        {
            KeyValuePair<bool, string> retornoConnection = new KeyValuePair<bool, string>();
            try
            {
                using (var context = contexto.CreateDbContext())
                {
                    bool testeConexao = context.Database.CanConnect();
                    retornoConnection = new KeyValuePair<bool, string>(testeConexao, "");
                }
            }
            catch (Exception e)
            {
                retornoConnection = new KeyValuePair<bool, string>(false, e.Message);

            }
            return retornoConnection;
        }

        // Recebe um keyValuePair Valor da equerda sao dados corretos prontos para insert 
        // valor da direita sao dados para colocar no log 
        public KeyValuePair<List<ITFEmpregado>, List<ITFEmpregado>> ValidaArquivoUploadEmpregado(string nomeArquivo, DataTable dados)
        {
            KeyValuePair<List<ITFEmpregado>, List<ITFEmpregado>> RetornoDados = new KeyValuePair<List<ITFEmpregado>, List<ITFEmpregado>>();

            List<ITFEmpregado> ListaImportacaoEmpregados = new List<ITFEmpregado>();
            List<ITFEmpregado> ErroValidacoesImport = new List<ITFEmpregado>();

            int numeroLinha = 2;

            foreach (DataRow linhaTabela in dados.Rows)
            {
                var quantidadeColunas = linhaTabela.ItemArray.ToList();
                ITFEmpregado modeloDados = new ITFEmpregado();
                string mensagemErro = "";

                var validaBeforeParseObject = contextoMontaObject.validaObjetoTransitoria(quantidadeColunas, numeroLinha);

                modeloDados = contextoMontaObject.montaObjetoTransitoria(quantidadeColunas);



                // Valida Cnpj Matriz
                if (modeloDados.CnpjMatriz.Trim() == "" || modeloDados.CnpjMatriz.Trim() == null)
                {
                    mensagemErro += "* Informar o CNPJ da Matriz na linha " + numeroLinha + "\n \n ";
                }
                else
                {
                    List<PesquisaDapperEmpFil> verificaDadosCnpj;
                    string conectionstring = contexto.RetornaConectionStrings();
                    using (var ctx = contexto.CreateDbContext())
                    {
                        try
                        {
                            // Verifica CNPJ Matriz 
                            using (SqlConnection dbDapper = new SqlConnection(conectionstring))
                            {
                                dbDapper.Open();
                                string querySelect = "SELECT emp.IdCliente empresa, fil.IdFilial filial FROM Med.Filial fil LEFT JOIN Med.Cliente emp(NOLOCK) ON emp.IdEmpresa = fil.IdEmpresa AND emp.IdCliente = fil.IdCliente WHERE Adm.ufcTrataZeros(REPLACE(REPLACE(REPLACE(emp.CnpjCpf, '.', ''), '/', ''), '-', ''), 14, 0) = Adm.ufcTrataZeros(REPLACE(REPLACE(REPLACE('" + modeloDados.CnpjMatriz.Trim() + "', '.', ''), '/', ''), '-', ''), 14, 0)";
                                verificaDadosCnpj = dbDapper.Query<List<PesquisaDapperEmpFil>>(querySelect).First();
                                dbDapper.Dispose();
                            }
                        }
                        catch (Exception e)
                        {
                            mensagemErro += "* Cnpj Matriz nao encontrada na linha " + numeroLinha + "\n \n ";
                        }
                    }
                }


                if (modeloDados.CnpjFilial.Trim() == "" || modeloDados.CnpjFilial.Trim() == null)
                {
                    mensagemErro += "* Informar o CNPJ da Filial na linha " + numeroLinha + "\n \n";
                }
                else
                {
                    PesquisaDapperEmpFil verificaDadosCnpj;
                    string conectionstring = contexto.RetornaConectionStrings();
                    using (var ctx = contexto.CreateDbContext())
                    {
                        try
                        {
                            // Verifica CNPJ Matriz 
                            using (SqlConnection dbDapper = new SqlConnection(conectionstring))
                            {
                                dbDapper.Open();
                                string querySelect = "SELECT emp.IdCliente empresa, fil.IdFilial filial FROM Med.Filial fil LEFT JOIN Med.Cliente emp(NOLOCK) ON emp.IdEmpresa = fil.IdEmpresa AND emp.IdCliente = fil.IdCliente WHERE Adm.ufcTrataZeros(REPLACE(REPLACE(REPLACE(fil.CnpjCpf, '.', ''), '/', ''), '-', ''), 14, 0) = Adm.ufcTrataZeros(REPLACE(REPLACE(REPLACE('" + modeloDados.CnpjFilial.Trim() + "', '.', ''), '/', ''), '-', ''), 14, 0)";
                                verificaDadosCnpj = dbDapper.Query<PesquisaDapperEmpFil>(querySelect).First();
                                dbDapper.Dispose();
                            }
                        }
                        catch (Exception e)
                        {
                            mensagemErro += "* Cnpj Filial nao encontrada na linha " + numeroLinha + "\n \n ";
                        }
                    }
                }

                if (modeloDados.CodCargo.Trim() == "" || modeloDados.CodCargo.Trim() == null)
                {
                    mensagemErro += "* Informar o Código do Cargo na linha " + numeroLinha + "\n \n";
                }
                if (modeloDados.NomeCargo.Trim() == "" || modeloDados.NomeCargo.Trim() == null)
                {
                    mensagemErro += "* Informar Nome do Cargo na linha " + numeroLinha + "\n \n";
                }
                if (modeloDados.CodSetor.Trim() == "" || modeloDados.CodSetor.Trim() == null)
                {
                    mensagemErro += "* Informar o Código do Setor  na linha " + numeroLinha + "\n \n";
                }
                if (modeloDados.NomeSetor.Trim() == "" || modeloDados.NomeSetor.Trim() == null)
                {
                    mensagemErro += "* Informar Nome do Setor na linha " + numeroLinha + "\n \n";
                }
                if (modeloDados.NumMatricula.Trim() == "" || modeloDados.NumMatricula.Trim() == null)
                {
                    mensagemErro += "* Informar o Número de Registro / Matricula  na linha " + numeroLinha + "\n \n";
                }

                if (modeloDados.DtAdmissao == DateTime.MinValue || modeloDados.DtAdmissao == null)
                {
                    mensagemErro += "* Informar a Data de Admissão na linha " + numeroLinha + "\n \n";
                }

                if (modeloDados.DtNascimento == DateTime.MinValue || modeloDados.DtNascimento == null)
                {
                    mensagemErro += "* Informar a Data de Nascimento na linha " + numeroLinha + "\n \n";
                }

                if (modeloDados.NomeFuncionario.Trim() == "" || modeloDados.NomeFuncionario.Trim() == null)
                {
                    mensagemErro += "* Informar o Nome Completo do Funcionário na linha " + numeroLinha + "\n \n";
                }

                if (modeloDados.CpfRne.Trim() == "" || modeloDados.CpfRne.Trim() == null)
                {
                    mensagemErro += "* Informar o Nº de CPF ou RNE na linha " + numeroLinha + "\n \n";
                }
                if (modeloDados.RgNumero.Trim() == "" || modeloDados.RgNumero.Trim() == null)
                {
                    mensagemErro += "* Informar o Nº do RG na linha " + numeroLinha + "\n \n";
                }

                //if (modeloDados.EstadoCivil.Trim() != "" && modeloDados.EstadoCivil.Trim() != null)
                //{
                //    if (modeloDados.EstadoCivil.ToUpper().Trim() != "C" &&
                //        modeloDados.EstadoCivil.ToUpper().Trim() != "S" &&
                //        modeloDados.EstadoCivil.ToUpper().Trim() != "V" &&
                //        modeloDados.EstadoCivil.ToUpper().Trim() != "D" &&
                //        modeloDados.EstadoCivil.ToUpper().Trim() != "U")
                //    {
                //        mensagemErro += "* Informar  na linha " + numeroLinha + " somente os seguintes dados - C = CASADO, S = SOLTEIRO, V = VIÚVO, D = DIVORCIADO, U = UNIÃO ESTÁVEL \n \n";
                //    }
                //}
                //else
                //{
                //    mensagemErro += "* Informar o estado civil  na linha " + numeroLinha + " valores aceitaveis : C = CASADO, S = SOLTEIRO, V = VIÚVO, D = DIVORCIADO, U = UNIÃO ESTÁVEL  \n \n";
                //}


                if (modeloDados.Sexo.Trim() != "" && modeloDados.Sexo.Trim() != null)
                {
                    if (modeloDados.Sexo.ToUpper().Trim() != "M" && modeloDados.Sexo.ToUpper().Trim() != "F")
                    {
                        mensagemErro += "* Informar somente M - Masculino e F - Feminino na linha " + numeroLinha + "\n \n ";
                    }
                }
                else
                {
                    mensagemErro += "* Informar o Sexo do Funcionário na linha " + numeroLinha + "\n \n ";
                }


                //if (modeloDados.NumPisNit.Trim() == "" || modeloDados.NumPisNit.Trim() == null)
                //{
                //    mensagemErro += "* Informar o Número do PIS/NIT na linha " + numeroLinha + "\n \n ";
                //}


                if (modeloDados.TpContratacao.Trim() != "" && modeloDados.TpContratacao.Trim() != null)
                {
                    if (
                        modeloDados.TpContratacao.Replace("0", "").Trim() != "1" &&
                        modeloDados.TpContratacao.Replace("0", "").Trim() != "2" &&
                        modeloDados.TpContratacao.Replace("0", "").Trim() != "3" &&
                        modeloDados.TpContratacao.Replace("0", "").Trim() != "4" &&
                        modeloDados.TpContratacao.Replace("0", "").Trim() != "5" &&
                        modeloDados.TpContratacao.Replace("0", "").Trim() != "6" &&
                        modeloDados.TpContratacao.Replace("0", "").Trim() != "7" &&
                        modeloDados.TpContratacao.Replace("0", "").Trim() != "8" &&
                        modeloDados.TpContratacao.Replace("0", "").Trim() != "9" &&
                        modeloDados.TpContratacao.Replace("0", "").Trim() != "10" &&
                        modeloDados.TpContratacao.Replace("0", "").Trim() != "11"
                        )
                    {
                        mensagemErro += "* Informar na linha " + numeroLinha + " tipo contratacao somente como mostra exemplo - Ex: 1 = CARTEIRA ASSINADA - CLT, 2 = ESTÁGIO, 3 = JOVEM APRENDIZ, 4 = CONTRATAÇÃO TEMPORÁRIA, 5 = TERCERIZAÇÃO, 6 = HOME OFFICE OU TRABALHO REMOTO, 7 = TRABALHO INTERMITENTE, 8 = TRABALHADOR EVENTUAL, 9 = TRABALHADOR AUTÔNOMO, 10 = CONTRATO DE EXPERIÊNCIA, 11 = TRABALHO EM REGIME DE TEMPO PARCIAL";
                    }
                }
                //else
                //{
                //    mensagemErro += "* Informar o Código para o Tipo de Contratação na linha " + numeroLinha + "\n \n ";
                //}




                //if (modeloDados.CategTrabalhador.Trim() == "" || modeloDados.CategTrabalhador.Trim() == null)
                //{
                //    mensagemErro += "* Informar Código da Categoria do Trabalhador de acordo com Tabela 01 do eSocial na linha " + numeroLinha + "\n \n ";
                //}


                if (modeloDados.NecessidadeEspecial.Trim() != "" && modeloDados.NecessidadeEspecial.Trim() != null)
                {
                    if (
                        modeloDados.NecessidadeEspecial.Replace("0", "").Trim() != "1" &&
                        modeloDados.NecessidadeEspecial.Replace("0", "").Trim() != "2" &&
                        modeloDados.NecessidadeEspecial.Replace("0", "").Trim() != "3" &&
                        modeloDados.NecessidadeEspecial.Replace("0", "").Trim() != "4" &&
                        modeloDados.NecessidadeEspecial.Replace("0", "").Trim() != "5" &&
                        modeloDados.NecessidadeEspecial.Replace("0", "").Trim() != "6" &&
                        modeloDados.NecessidadeEspecial.Replace("0", "").Trim() != "7"
                        )
                    {
                        mensagemErro += "* Informar na linha " + numeroLinha + " Necessidade especial como somente como mostra exemplo -  Ex.: 1 = Nenhuma, 2 = Auditiva, 3=Física, 4 = Intelectual, 5 = Multipla, 6 = Visual, 7 = Reabilitado Inss. \n \n";
                    }

                }
                //else
                //{
                //    mensagemErro += "* Informar NecessidadeEspecial na linha " + numeroLinha + "\n \n ";
                //}

                if (modeloDados.Situacao.Trim() != "" && modeloDados.Situacao.Trim() != null)
                {
                    if (
                           modeloDados.Situacao.Replace("0", "").Trim() != "1" &&
                           modeloDados.Situacao.Replace("0", "").Trim() != "2" &&
                           modeloDados.Situacao.Replace("0", "").Trim() != "3" &&
                           modeloDados.Situacao.Replace("0", "").Trim() != "4" &&
                           modeloDados.Situacao.Replace("0", "").Trim() != "5" &&
                           modeloDados.Situacao.Replace("0", "").Trim() != "6"
                       )
                    {
                        mensagemErro += "* Informar na linha " + numeroLinha + " somente as Situação do Empregado como exemplo - Ex.: 1=Admitido, 2=Demitido, 3=Afastado, 4=Licença Maternidade, 5=Aposentado, 6=Candidato  \n \n";

                    }
                }
                else
                {
                    mensagemErro += "* Informar a Situação do Empregado na linha " + numeroLinha + "\n \n ";
                }



                if (modeloDados.DescrAtividade.Trim() == "" || modeloDados.DescrAtividade.Trim() == null)
                {
                    mensagemErro += "* Descrever a atividade exercida pelo funcionário na linha " + numeroLinha + "\n \n ";
                }



                if (modeloDados.AdicInsalubridade.Trim() != "" && modeloDados.AdicInsalubridade.Trim() != null)
                {
                    if (
                            modeloDados.AdicInsalubridade.Trim().ToUpper() != "S" &&
                            modeloDados.AdicInsalubridade.Trim().ToUpper() != "N"
                       )
                    {
                        mensagemErro += "* Informar adicional insalubridade somente com S - Sim , N - Não na linha  " + numeroLinha + " \n \n ";

                    }
                }
                //else
                //{
                //    mensagemErro += "* Informar adicional insalubridade na linha " + numeroLinha + "\n \n ";
                //}


                if (modeloDados.AdicPericulosidade.Trim() != "" && modeloDados.AdicPericulosidade.Trim() != null)
                {
                    if (
                            modeloDados.AdicPericulosidade.Trim().ToUpper() != "S" &&
                            modeloDados.AdicPericulosidade.Trim().ToUpper() != "N"
                       )
                    {
                        mensagemErro += "* Informar adicional Periculosidade somente com S - Sim , N - Não na linha  " + numeroLinha + " \n \n ";

                    }
                }
                //else
                //{
                //    mensagemErro += "* Informar adicional Periculosidade na linha " + numeroLinha + "\n \n ";
                //}

                if (modeloDados.Lotacao.Trim() != "" && modeloDados.Lotacao.Trim() != null)
                {
                    if (
                            modeloDados.Lotacao.Replace("0", "").Trim() != "1" &&
                            modeloDados.Lotacao.Replace("0", "").Trim() != "2" &&
                            modeloDados.Lotacao.Replace("0", "").Trim() != "3"
                       )
                    {
                        mensagemErro += "* Informar somente 1 = Estabelecimento do empregador , 2 = Estabelecimento de terceiros , 3 = Prestação de serviços   na linha  " + numeroLinha + " \n \n ";

                    }
                }

                //else
                //{
                //    mensagemErro += "* Informar Lotação na linha " + numeroLinha + "\n \n ";
                //}

                //if (modeloDados.CEP.Trim() == "" || modeloDados.CEP.Trim() == null)
                //{
                //    mensagemErro += "* Informar CEP na linha " + numeroLinha + " \n \n ";
                //}

                //if (modeloDados.TpLogradouro.Trim() == "" || modeloDados.TpLogradouro.Trim() == null)
                //{
                //    mensagemErro += "* Informar Tipo Logradouro na linha " + numeroLinha + " \n \n ";
                //}

                //if (modeloDados.Numero.Trim() == "" || modeloDados.Numero.Trim() == null)
                //{
                //    mensagemErro += "* Informar Numero do Endereço na linha " + numeroLinha + " \n \n ";
                //}

                //if (modeloDados.Bairro.Trim() == "" || modeloDados.Bairro.Trim() == null)
                //{
                //    mensagemErro += "* Informar Bairro na linha " + numeroLinha + " \n \n ";
                //}
                //if (modeloDados.UF.Trim() == "" || modeloDados.UF.Trim() == null)
                //{
                //    mensagemErro += "* Informar a UF - Unidade da Federação na linha " + numeroLinha + " \n \n ";
                //}
                //if (modeloDados.Cidade.Trim() == "" || modeloDados.Cidade.Trim() == null)
                //{
                //    mensagemErro += "* Informar a Cidade na linha " + numeroLinha + " \n \n ";
                //}

                //if (modeloDados.Pais.Trim() == "" || modeloDados.Pais.Trim() == null)
                //{
                //    mensagemErro += "* Informar o País na linha " + numeroLinha + " \n \n ";
                //}

                if (modeloDados.RgUF.Length > 2)
                {
                    mensagemErro += "* Quantidade de caracteres excedidos  no campo RgUF  , campo deve ter apenas 2 caracteres na  linha " + numeroLinha + " \n \n ";
                }

                if (modeloDados.Sexo.Length > 3)
                {
                    mensagemErro += "* Quantidade de caracteres excedidos no campo Sexo  , campo deve ter apenas 3 caracteres na  linha " + numeroLinha + " \n \n ";
                }

                if (modeloDados.TpContratacao.Length > 2)
                {
                    mensagemErro += "* Quantidade de caracteres excedidos no campo TpContratacao  , campo deve ter apenas 2 caracteres na  linha " + numeroLinha + " \n \n ";
                }

                if (modeloDados.AdicInsalubridade.Length > 3)
                {
                    mensagemErro += "* Quantidade de caracteres excedidos no campo AdicInsalubridade  , campo deve ter apenas 3 caracteres na  linha " + numeroLinha + " \n \n ";
                }

                if (modeloDados.AdicPericulosidade.Length > 1)
                {
                    mensagemErro += "* Quantidade de caracteres excedidos no campo AdicPericulosidade  , campo deve ter apenas 1 caracter na  linha " + numeroLinha + " \n \n ";
                }

                if (modeloDados.Lotacao.Length > 1)
                {
                    mensagemErro += "* Quantidade de caracteres excedidos no campo Lotacao  , campo deve ter apenas 1 caracter na  linha " + numeroLinha + " \n \n ";
                }

                if (modeloDados.NecessidadeEspecial.Length > 2)
                {
                    mensagemErro += "* Quantidade de caracteres excedidos no campo NecessidadeEspecial  , campo deve ter apenas 2 caracteres na  linha " + numeroLinha + " \n \n ";
                }

                if (modeloDados.Situacao.Length > 2)
                {
                    mensagemErro += "* Quantidade de caracteres excedidos no campo Situacao  , campo deve ter apenas 2 caracteres na  linha " + numeroLinha + " \n \n ";
                }

                if (modeloDados.UF.Length > 2)
                {
                    mensagemErro += "* Quantidade de caracteres excedidos no campo UF  , campo deve ter apenas 2 caracteres na  linha " + numeroLinha + " \n \n ";
                }

                if (modeloDados.TpDependente.Length > 2)
                {
                    mensagemErro += "* Quantidade de caracteres excedidos no campo TpDependente  , campo deve ter apenas 2 caracteres na  linha " + numeroLinha + " \n \n ";
                }

                if (validaBeforeParseObject.Key == false)
                {
                    mensagemErro += validaBeforeParseObject.Value;
                }

                modeloDados.NumLinha = numeroLinha;
                modeloDados.CdChaveAcao = nomeArquivo;

                if (mensagemErro.Length > 0)
                {
                    mensagemErro += " \n \n ---------------------------------------------------------- \n \n ";

                    modeloDados.DtChaveAcao = DateTime.Now;
                    modeloDados.MsgCritica = mensagemErro;
                    ErroValidacoesImport.Add(modeloDados);
                }
                else
                {
                    ListaImportacaoEmpregados.Add(modeloDados);
                }

                numeroLinha++;
            }

            RetornoDados = new KeyValuePair<List<ITFEmpregado>, List<ITFEmpregado>>(ListaImportacaoEmpregados, ErroValidacoesImport);

            return RetornoDados;

        }

        public bool montaLogRegistro(string nomeArquivo, List<ITFEmpregado> listaLogErro, string caminhoProjeto)
        {
            bool retorno = false;

            try
            {
                StreamWriter arquivoText;
                string caminhoDiretorio = caminhoProjeto + @"\ArquivosUpload\Logs_de_erros\";
                string caminhoArquivoCompleto = caminhoDiretorio + "Log_de_validacao_" + nomeArquivo + ".txt";
                if (!Directory.Exists(caminhoDiretorio))
                {
                    Directory.CreateDirectory(caminhoDiretorio);
                }
                //Cria arquivo de texto
                arquivoText = File.CreateText(caminhoArquivoCompleto);
                foreach (ITFEmpregado linha in listaLogErro)
                {
                    if (linha.MsgCritica != "")
                    {
                        arquivoText.WriteLine(linha.MsgCritica);
                    }
                }
                arquivoText.Close();
            }
            catch (Exception)
            {
                return retorno;
            }
            retorno = true;
            return retorno;
        }


        public bool montaLogInsertDadosEmpregados(string nomeArquivo, RetornoCargaFuncionarios listaLogErro, string caminhoProjeto)
        {
            bool retorno = false;
            try
            {
                StreamWriter arquivoText;
                string caminhoDiretorio = caminhoProjeto + @"\ArquivosUpload\Logs_de_inserts\";
                string caminhoArquivoCompleto = caminhoDiretorio + "insert_empregados_" + DateTime.Now.ToShortDateString().Replace("/", "_") + "_" + nomeArquivo + ".txt";

                if (!Directory.Exists(caminhoDiretorio))
                {
                    Directory.CreateDirectory(caminhoDiretorio);
                }

                //Cria arquivo de texto
                arquivoText = File.CreateText(caminhoArquivoCompleto);

                if (listaLogErro.QtdCargoUpload.Value != 0)
                {
                    arquivoText.WriteLine("CARGOS INSERIDOS : \n \n ");

                    foreach (Cargo cargosInseridos in listaLogErro.QtdCargoUpload.Key)
                    {
                        arquivoText.WriteLine(" * Nome Cargo : " + cargosInseridos.Nome + "\n" + "* ID cargo : " + cargosInseridos.IdCargo + "\n \n ");

                    }
                    arquivoText.WriteLine(" -------------------------------------------------------------------- ");
                }

                if (listaLogErro.QtdCentroCustoUpload.Value != 0)
                {
                    arquivoText.WriteLine("CENTRO DE CUSTOS INSERIDOS : \n \n ");
                    foreach (CentroDeCusto centrodeCustos in listaLogErro.QtdCentroCustoUpload.Key)
                    {
                        arquivoText.WriteLine(" * Nome Centro de custo : " + centrodeCustos.Nome + "\n" + "* ID Centro de custo : " + centrodeCustos.IdCentroCusto + "\n \n ");
                    }
                    arquivoText.WriteLine(" -------------------------------------------------------------------- ");
                }

                if (listaLogErro.QtdSetorUpload.Value != 0)
                {
                    arquivoText.WriteLine("SETORES  INSERIDOS : \n \n ");
                    foreach (Setor setores in listaLogErro.QtdSetorUpload.Key)
                    {
                        arquivoText.WriteLine(" * Nome Setor : " + setores.Nome + "\n" + "* ID Setor : " + setores.IdSetor + "\n \n ");
                    }
                    arquivoText.WriteLine(" -------------------------------------------------------------------- ");
                }

                if (listaLogErro.QtdEmpregadosInseridos.Value != 0)
                {
                    arquivoText.WriteLine("EMPREGADOS  INSERIDOS : \n \n ");
                    foreach (Empregados empregadosinseridos in listaLogErro.QtdEmpregadosInseridos.Key)
                    {
                        arquivoText.WriteLine(" * Nome Empregado : " + empregadosinseridos.Nome + "\n" + "* ID Empregado : " + empregadosinseridos.IdEmpregado + "\n \n ");
                    }
                    arquivoText.WriteLine(" -------------------------------------------------------------------- ");
                }

                if (listaLogErro.QtdEmpregadosAtualizados.Value != 0)
                {
                    arquivoText.WriteLine("EMPREGADOS  ATUALIZADOS : \n \n ");
                    foreach (Empregados empregadosatualizados in listaLogErro.QtdEmpregadosAtualizados.Key)
                    {
                        arquivoText.WriteLine(" * Nome Empregado: " + empregadosatualizados.Nome + "\n" + "* ID Empregado : " + empregadosatualizados.IdEmpregado + "\n \n ");
                    }

                    arquivoText.WriteLine(" -------------------------------------------------------------------- ");
                }

                if (listaLogErro.ErrosEmpregados.Count() > 0)
                {
                    arquivoText.WriteLine("ERROS INSERÇÃO DE DADOS EMPREGADOS : \n \n ");
                    foreach (KeyValuePair<Empregados, string> validacoes in listaLogErro.ErrosEmpregados)
                    {
                        arquivoText.WriteLine("Empregado " + validacoes.Key.Nome + " " + " Id Empregado " + validacoes.Key.IdEmpregado + "\n \n ");
                    }
                    arquivoText.WriteLine(" -------------------------------------------------------------------- ");
                }

                arquivoText.Close();
            }
            catch (Exception e)
            {
                return retorno;
            }

            retorno = true;
            return retorno;
        }

        public async void gravaDadosTransitoria(List<ITFEmpregado> listaLogErros)
        {
            //int  retorno = 0;
            string QueryExecute = "";
            using (var ctx = contexto.CreateDbContext())
            {
                var valorNuloQuery = DBNull.Value;
                var GeralTabela = ctx.TransitoriaEmpregados.ToList();

                string conectionString = contexto.RetornaConectionStrings();

                using (SqlConnection dbDapper = new SqlConnection(conectionString))
                {
                    foreach (ITFEmpregado logError in listaLogErros)
                    {
                        ITFEmpregado RegistroBanco = new ITFEmpregado();
                        try
                        {
                            RegistroBanco = GeralTabela.Where(r => r.CdChaveAcao.Trim() == logError.CdChaveAcao.Trim() && r.NumLinha == logError.NumLinha).First();
                        }
                        catch (Exception e)
                        {

                            RegistroBanco = null;
                        }

                        // monta insert na lista caso nao encontre o dado cadastrado no banco
                        if (RegistroBanco == null)
                        {
                            QueryExecute += "INSERT INTO Med.ITF_Empregado VALUES(" + (logError.CdChaveAcao.Trim() == null ? "NULL" : "'" + logError.CdChaveAcao.Trim() + "'") + ","
                                                                                    + (logError.DtChaveAcao.Value.GetDateTimeFormats()[47] == null ? "NULL" : "'" + logError.DtChaveAcao.Value.GetDateTimeFormats()[47] + "'") + ","
                                                                                    + (logError.NumLinha == null ? "NULL" : logError.NumLinha.ToString()) + ","
                                                                                    + (logError.CodMatriz == null ? "NULL" : logError.CodMatriz.ToString()) + ","
                                                                                    + (logError.NomeMatriz.Trim() == null ? "NULL" : "'" + logError.NomeMatriz.Trim() + "'") + ","
                                                                                    + (logError.CnpjMatriz.Trim() == null ? "NULL" : "'" + logError.CnpjMatriz.Trim() + "'") + ","
                                                                                    + (logError.CodFilial == null ? "NULL" : logError.CodFilial.ToString()) + ","
                                                                                    + (logError.RazaoSocial.Trim() == null ? "NULL" : "'" + logError.RazaoSocial.Trim() + "'") + ","
                                                                                    + (logError.CnpjFilial.Trim() == null ? "NULL" : "'" + logError.CnpjFilial.Trim() + "'") + ","
                                                                                    + (logError.CodGHE.Trim() == null ? "NULL" : "'" + logError.CodGHE.Trim() + "'") + ","
                                                                                    + (logError.NomeGHE.Trim() == null ? "NULL" : "'" + logError.NomeGHE.Trim() + "'") + ","
                                                                                    + (logError.CodCargo.Trim() == null ? "NULL" : "'" + logError.CodCargo.Trim() + "'") + ","
                                                                                    + (logError.NomeCargo.Trim() == null ? "NULL" : "'" + logError.NomeCargo.Trim() + "'") + ","
                                                                                    + (logError.CodSetor.Trim() == null ? "NULL" : "'" + logError.CodSetor.Trim() + "'") + ","
                                                                                    + (logError.NomeSetor.Trim() == null ? "NULL" : "'" + logError.NomeSetor.Trim() + "'") + ","
                                                                                    + (logError.CodCentroCusto.Trim() == null ? "NULL" : "'" + logError.CodCentroCusto.Trim() + "'") + ","
                                                                                    + (logError.NomeCentroCusto.Trim() == null ? "NULL" : "'" + logError.NomeCentroCusto.Trim() + "'") + ","
                                                                                    + (logError.NumMatricula.Trim() == null ? "NULL" : "'" + logError.NumMatricula.Trim() + "'") + ","
                                                                                    + (logError.NumChapa.Trim() == null ? "NULL" : "'" + logError.NumChapa.Trim() + "'") + ","
                                                                                    + (logError.DtAdmissao == null ? "NULL" : "'" + logError.DtAdmissao.Value.GetDateTimeFormats()[13] + "'") + ","
                                                                                    + (logError.DtDemissao == null ? "NULL" : "'" + logError.DtDemissao.Value.GetDateTimeFormats()[13] + "'") + ","
                                                                                    + (logError.DtNascimento == null ? "NULL" : "'" + logError.DtNascimento.Value.GetDateTimeFormats()[13] + "'") + ","
                                                                                    + (logError.NomeFuncionario.Trim() == null ? "NULL" : "'" + logError.NomeFuncionario.Trim() + "'") + ","
                                                                                    + (logError.CpfRne.Trim() == null ? "NULL" : "'" + logError.CpfRne.Trim() + "'") + ","
                                                                                    + (logError.RgUF.Trim() == null ? "NULL" : "'" + logError.RgUF.Trim() + "'") + ","
                                                                                    + (logError.RgNumero.Trim() == null ? "NULL" : "'" + logError.RgNumero.Trim() + "'") + ",'"
                                                                                    + logError.RgOrgaoEmissor.Trim() + "',"
                                                                                    + (logError.RgDtEmissao == null ? "NULL" : "'" + logError.RgDtEmissao.Value.GetDateTimeFormats()[13] + "'") + ",'"
                                                                                    + logError.CtpsUf.Trim() + "','"
                                                                                    + logError.CtpsNumero.Trim() + "','"
                                                                                    + logError.CtpsSerie.Trim() + "',"
                                                                                    + (logError.CtpsDtEmissao == null ? "" : "'" + logError.CtpsDtEmissao.Value.GetDateTimeFormats()[13] + "'") + ","
                                                                                    + (logError.EstadoCivil.Trim() == null ? "NULL" : "'" + logError.EstadoCivil.Trim() + "'") + ","
                                                                                    + (logError.Sexo.Trim() == null ? "NULL" : "'" + logError.Sexo.Trim() + "'") + ","
                                                                                    + (logError.NumPisNit.Trim() == null ? "NULL" : "'" + logError.NumPisNit.Trim() + "'") + ","
                                                                                    + (logError.NumGfip.Trim() == null ? "NULL" : "'" + logError.NumGfip.Trim() + "'") + ","
                                                                                    + (logError.TpContratacao.Trim() == null ? "NULL" : "'" + logError.TpContratacao.Trim() + "'") + ","
                                                                                    + (logError.CategTrabalhador.Trim() == null ? "NULL" : "'" + logError.CategTrabalhador.Trim() + "'") + ","
                                                                                    + (logError.NecessidadeEspecial.Trim() == null ? "NULL" : "'" + logError.NecessidadeEspecial.Trim() + "'") + ","
                                                                                    + (logError.Situacao.Trim() == null ? "NULL" : "'" + logError.Situacao.Trim() + "'") + ","
                                                                                    + (logError.DescrAtividade.Trim() == null ? "NULL" : "'" + logError.DescrAtividade.Trim() + "'") + ","
                                                                                    + (logError.NomeSupDireto.Trim() == null ? "NULL" : "'" + logError.NomeSupDireto.Trim() + "'") + ","
                                                                                    + (logError.EmailSupDireto.Trim() == null ? "NULL" : "'" + logError.EmailSupDireto.Trim() + "'") + ","
                                                                                    + (logError.AdicInsalubridade.Trim() == null ? "NULL" : "'" + logError.AdicInsalubridade.Trim() + "'") + ","
                                                                                    + (logError.GrauInsalubridade.Trim() == null ? "NULL" : "'" + logError.GrauInsalubridade.Trim() + "'") + ","
                                                                                    + (logError.AdicPericulosidade.Trim() == null ? "NULL" : "'" + logError.AdicPericulosidade.Trim() + "'") + ","
                                                                                    + (logError.GrauPericulosidade.Trim() == null ? "NULL" : "'" + logError.GrauPericulosidade.Trim() + "'") + ","
                                                                                    + (logError.NumCBO.Trim() == null ? "NULL" : "'" + logError.NumCBO.Trim() + "'") + ","
                                                                                    + (logError.DescrTurnoTrabalho.Trim() == null ? "NULL" : "'" + logError.DescrTurnoTrabalho.Trim() + "'") + ","
                                                                                    + (logError.Lotacao.Trim() == null ? "NULL" : "'" + logError.Lotacao.Trim() + "'") + ","
                                                                                    + (logError.CEP.Trim() == null ? "NULL" : "'" + logError.CEP.Trim() + "'") + ","
                                                                                    + (logError.TpLogradouro.Trim() == null ? "NULL" : "'" + logError.TpLogradouro.Trim() + "'") + ","
                                                                                    + (logError.Logradouro.Trim() == null ? "NULL" : "'" + logError.Logradouro.Trim() + "'") + ","
                                                                                    + (logError.Numero.Trim() == null ? "NULL" : "'" + logError.Numero.Trim() + "'") + ","
                                                                                    + (logError.Complemento.Trim() == null ? "NULL" : "'" + logError.Complemento.Trim() + "'") + ","
                                                                                    + (logError.Bairro.Trim() == null ? "NULL" : "'" + logError.Bairro.Trim() + "'") + ","
                                                                                    + (logError.UF.Trim() == null ? "NULL" : "'" + logError.UF.Trim() + "'") + ","
                                                                                    + (logError.Cidade.Trim() == null ? "NULL" : "'" + logError.Cidade.Trim() + "'") + ","
                                                                                    + (logError.Pais.Trim() == null ? "NULL" : "'" + logError.Pais.Trim() + "'") + ","
                                                                                    + (logError.Email.Trim() == null ? "NULL" : "'" + logError.Email.Trim() + "'") + ","
                                                                                    + (logError.Telefone.Trim() == null ? "NULL" : "'" + logError.Telefone.Trim() + "'") + ","
                                                                                    + (logError.Celular.Trim() == null ? "NULL" : "'" + logError.Celular.Trim() + "'") + ","
                                                                                    + (logError.NomeDependente.Trim() == null ? "NULL" : "'" + logError.NomeDependente.Trim() + "'") + ","
                                                                                    + (logError.CpfDependente.Trim() == null ? "NULL" : "'" + logError.CpfDependente.Trim() + "'") + ","
                                                                                    + (logError.TpDependente.Trim() == null ? "NULL" : "'" + logError.TpDependente.Trim() + "'") + ",'"
                                                                                    + logError.MsgCritica.Trim() + "',"
                                                                                    + (logError.DtCritica == null ? "NULL" : "'" + logError.DtCritica.Value.ToShortDateString() + "'") + ","
                                                                                    + 1 + ","
                                                                                    + (logError.IdEmpregado == null ? "NULL" : logError.IdEmpregado.ToString()) + "); ";

                        }
                        //monta update na lista caso encontre o dado cadastrado no banco
                        else
                        {
                            QueryExecute += "UPDATE Med.ITF_Empregado SET " +
                                            " CdChaveAcao = " + (logError.CdChaveAcao.Trim() == null ? "NULL" : "'" + logError.CdChaveAcao.Trim()) + "'" + "," +
                                            " DtChaveAcao = " + (logError.DtChaveAcao.Value.GetDateTimeFormats()[47] == null ? "NULL" : "'" + logError.DtChaveAcao.Value.GetDateTimeFormats()[47] + "'") + "," +
                                            " NumLinha =" + (logError.NumLinha == null ? "NULL" : logError.NumLinha.ToString()) + "," +
                                            " CodMatriz =" + (logError.CodMatriz == null ? "NULL" : logError.CodMatriz.ToString()) + "," +
                                            " NomeMatriz = " + (logError.NomeMatriz.Trim() == null ? "NULL" : "'" + logError.NomeMatriz.Trim() + "'") + ", " +
                                            " CnpjMatriz =" + (logError.CnpjMatriz.Trim() == null ? "NULL" : "'" + logError.CnpjMatriz.Trim() + "'") + ", " +
                                            " CodFilial = " + (logError.CodFilial == null ? "NULL" : logError.CodFilial.ToString()) + "," +
                                            " RazaoSocial = " + (logError.RazaoSocial.Trim() == null ? "NULL" : "'" + logError.RazaoSocial.Trim() + "'") + "," +
                                            " CnpjFilial = " + (logError.CnpjFilial.Trim() == null ? "NULL" : "'" + logError.CnpjFilial.Trim() + "'") + "," +
                                            " CodGHE = " + (logError.CodGHE.Trim() == null ? "NULL" : "'" + logError.CodGHE.Trim() + "'") + "," +
                                            " NomeGHE = " + (logError.NomeGHE.Trim() == null ? "NULL" : "'" + logError.NomeGHE.Trim() + "'") + "," +
                                            " CodCargo = " + (logError.CodCargo.Trim() == null ? "NULL" : "'" + logError.CodCargo.Trim() + "'") + "," +
                                            " NomeCargo = " + (logError.NomeCargo.Trim() == null ? "NULL" : "'" + logError.NomeCargo.Trim() + "'") + "," +
                                            " CodSetor = " + (logError.CodSetor.Trim() == null ? "NULL" : "'" + logError.CodSetor.Trim() + "'") + "," +
                                            " NomeSetor = " + (logError.NomeSetor.Trim() == null ? "NULL" : "'" + logError.NomeSetor.Trim() + "'") + "," +
                                            " CodCentroCusto = " + (logError.CodCentroCusto.Trim() == null ? "NULL" : "'" + logError.CodCentroCusto.Trim() + "'") + "," +
                                            " NomeCentroCusto = " + (logError.NomeCentroCusto.Trim() == null ? "NULL" : "'" + logError.NomeCentroCusto.Trim() + "'") + "," +
                                            " NumMatricula = " + (logError.NumMatricula.Trim() == null ? "NULL" : "'" + logError.NumMatricula.Trim() + "'") + "," +
                                            " NumChapa = " + (logError.NumChapa.Trim() == null ? "NULL" : "'" + logError.NumChapa.Trim() + "'") + "," +
                                            " DtAdmissao = " + (logError.DtAdmissao == null ? "NULL" : "'" + logError.DtAdmissao.Value.GetDateTimeFormats()[13] + "'") + "," +
                                            " DtDemissao = " + (logError.DtDemissao == null ? "NULL" : "'" + logError.DtDemissao.Value.GetDateTimeFormats()[13] + "'") + "," +
                                            " DtNascimento = " + (logError.DtNascimento == null ? "NULL" : "'" + logError.DtNascimento.Value.GetDateTimeFormats()[13] + "'") + "," +
                                            " NomeFuncionario = " + (logError.NomeFuncionario.Trim() == null ? "NULL" : "'" + logError.NomeFuncionario.Trim() + "'") + "," +
                                            " CpfRne = " + (logError.CpfRne.Trim() == null ? "NULL" : "'" + logError.CpfRne.Trim() + "'") + "," +
                                            " RgUF = " + (logError.RgUF.Trim() == null ? "NULL" : "'" + logError.RgUF.Trim() + "'") + ", " +
                                            " RgNumero = " + (logError.RgNumero.Trim() == null ? "NULL" : "'" + logError.RgNumero.Trim() + "'") + " , " +
                                            " RgOrgaoEmissor = " + (logError.RgOrgaoEmissor.Trim() == null ? "NULL" : "'" + logError.RgOrgaoEmissor.Trim() + "'") + " , " +
                                            " RgDtEmissao = " + (logError.RgDtEmissao == null ? "NULL" : "'" + logError.RgDtEmissao.Value.GetDateTimeFormats()[13] + "'") + ", " +
                                            " CtpsUf = " + (logError.CtpsUf.Trim() == null ? "NULL" : "'" + logError.CtpsUf.Trim() + "'") + "," +
                                            " CtpsNumero = " + (logError.CtpsNumero.Trim() == null ? "NULL" : "'" + logError.CtpsNumero.Trim() + "'") + " , " +
                                            " CtpsSerie = " + (logError.CtpsSerie.Trim() == null ? "NULL" : "'" + logError.CtpsSerie.Trim() + "'") + " , " +
                                            " CtpsDtEmissao = " + (logError.CtpsDtEmissao == null ? "NULL" : "'" + logError.CtpsDtEmissao.Value.GetDateTimeFormats()[13] + "'") + " , " +
                                            " EstadoCivil = " + (logError.EstadoCivil.Trim() == null ? "NULL" : "'" + logError.EstadoCivil.Trim() + "'") + " , " +
                                            " Sexo = " + (logError.Sexo.Trim() == null ? "NULL" : "'" + logError.Sexo.Trim() + "'") + " ," +
                                            " NumPisNit = " + (logError.NumPisNit.Trim() == null ? "NULL" : "'" + logError.NumPisNit.Trim() + "'") + "," +
                                            " NumGfip = " + (logError.NumGfip.Trim() == null ? "NULL" : "'" + logError.NumGfip.Trim() + "'") + " , " +
                                            " TpContratacao = " + (logError.TpContratacao.Trim() == null ? "NULL" : "'" + logError.TpContratacao.Trim() + "'") + " , " +
                                            " CategTrabalhador = " + (logError.CategTrabalhador.Trim() == null ? "NULL" : "'" + logError.CategTrabalhador.Trim() + "'") + " ," +
                                            " NecessidadeEspecial = " + (logError.NecessidadeEspecial.Trim() == null ? "NULL" : "'" + logError.NecessidadeEspecial.Trim() + "'") + "," +
                                            " Situacao = " + (logError.Situacao.Trim() == null ? "NULL" : "'" + logError.Situacao.Trim() + "'") + "," +
                                            " DescrAtividade = " + (logError.DescrAtividade.Trim() == null ? "NULL" : "'" + logError.DescrAtividade.Trim() + "'") + "," +
                                            " NomeSupDireto = " + (logError.NomeSupDireto.Trim() == null ? "NULL" : "'" + logError.NomeSupDireto.Trim() + "'") + "," +
                                            " EmailSupDireto = " + (logError.EmailSupDireto.Trim() == null ? "NULL" : "'" + logError.EmailSupDireto.Trim() + "'") + "," +
                                            " AdicInsalubridade = " + (logError.AdicInsalubridade.Trim() == null ? "NULL" : "'" + logError.AdicInsalubridade.Trim() + "'") + "," +
                                            " GrauInsalubridade = " + (logError.GrauInsalubridade.Trim() == null ? "NULL" : "'" + logError.GrauInsalubridade.Trim() + "'") + "," +
                                            " AdicPericulosidade = " + (logError.AdicPericulosidade.Trim() == null ? "NULL" : "'" + logError.AdicPericulosidade.Trim() + "'") + "," +
                                            " GrauPericulosidade = " + (logError.GrauPericulosidade.Trim() == null ? "NULL" : "'" + logError.GrauPericulosidade.Trim() + "'") + "," +
                                            " NumCBO = " + (logError.NumCBO.Trim() == null ? "NULL" : "'" + logError.NumCBO.Trim() + "'") + "," +
                                            " DescrTurnoTrabalho = " + (logError.DescrTurnoTrabalho.Trim() == null ? "NULL" : "'" + logError.DescrTurnoTrabalho.Trim() + "'") + "," +
                                            " Lotacao = " + (logError.Lotacao.Trim() == null ? "NULL" : "'" + logError.Lotacao.Trim() + "'") + "," +
                                            " CEP = " + (logError.CEP.Trim() == null ? "NULL" : "'" + logError.CEP.Trim() + "'") + "," +
                                            " TpLogradouro = " + (logError.TpLogradouro.Trim() == null ? "NULL" : "'" + logError.TpLogradouro.Trim() + "'") + "," +
                                            " Logradouro = " + (logError.Logradouro.Trim() == null ? "NULL" : "'" + logError.Logradouro.Trim() + "'") + "," +
                                            " Numero = " + (logError.Numero.Trim() == null ? "NULL" : "'" + logError.Numero.Trim() + "'") + "," +
                                            " Complemento = " + (logError.Complemento.Trim() == null ? "NULL" : "'" + logError.Complemento.Trim() + "'") + "," +
                                            " Bairro = " + (logError.Bairro.Trim() == null ? "NULL" : "'" + logError.Bairro.Trim() + "'") + "," +
                                            " UF = " + (logError.UF.Trim() == null ? "NULL" : "'" + logError.UF.Trim() + "'") + "," +
                                            " Cidade = " + (logError.Cidade.Trim() == null ? "NULL" : "'" + logError.Cidade.Trim() + "'") + "," +
                                            " Pais = " + (logError.Pais.Trim() == null ? "NULL" : "'" + logError.Pais.Trim() + "'") + "," +
                                            " Email = " + (logError.Email.Trim() == null ? "NULL" : "'" + logError.Email.Trim() + "'") + "," +
                                            " Telefone = " + (logError.Telefone.Trim() == null ? "NULL" : "'" + logError.Telefone.Trim() + "'") + "," +
                                            " Celular = " + (logError.Celular.Trim() == null ? "NULL" : "'" + logError.Celular.Trim() + "'") + "," +
                                            " NomeDependente = " + (logError.NomeDependente.Trim() == null ? "NULL" : "'" + logError.NomeDependente.Trim() + "'") + "," +
                                            " CpfDependente = " + (logError.CpfDependente.Trim() == null ? "NULL" : "'" + logError.CpfDependente.Trim() + "'") + "," +
                                            " TpDependente = " + (logError.TpDependente.Trim() == null ? "NULL" : "'" + logError.TpDependente.Trim() + "'") + "," +
                                            " MsgCritica = " + (logError.MsgCritica.Trim() == null ? "NULL" : "'" + logError.MsgCritica.Trim() + "'") + "," +
                                            " DtCritica = " + (logError.DtCritica == null ? "NULL" : "'" + logError.DtCritica.Value.ToShortDateString() + "'") + "," +
                                            " CtrlCritica =" + 1 + " , " +
                                            " IdEmpregado =" + (logError.IdEmpregado == null ? "NULL" : logError.IdEmpregado.ToString()) + "  WHERE CdChaveAcao = '" + logError.CdChaveAcao + "' AND NumLinha = " + logError.NumLinha + "   ";
                        }
                    }
                    // Executa todos os comandos em massa
                    dbDapper.Execute(QueryExecute);
                    dbDapper.Dispose();
                }
                ctx.Dispose();
            }
        }


        public RetornoCargaFuncionarios GravaDadosEmpregados(List<ITFEmpregado> DadosInclusao)
        {
            int QtdCargosInseridos = 0;
            int QtdSetorInseridos = 0;
            int QtdCentroCustoInseridos = 0;
            int QtdEmpregadosInseridos = 0;
            int QtdEmpregadosAtualizados = 0;
            List<Cargo> cargosInseridos = new List<Cargo>();
            List<Setor> SetoresInseridos = new List<Setor>();
            List<CentroDeCusto> CentroCustoInseridos = new List<CentroDeCusto>();
            List<Empregados> EmpregadosInseridos = new List<Empregados>();
            List<Empregados> EmpregadosAtualizados = new List<Empregados>();
            List<KeyValuePair<Empregados, string>> errosEmpregados = new List<KeyValuePair<Empregados, string>>();
            RetornoCargaFuncionarios retornoDadosGravacao = new RetornoCargaFuncionarios();

            using (var ctx = contexto.CreateDbContext())
            {
                foreach (ITFEmpregado dados in DadosInclusao)
                {
                    Empregados objetoAtual = new Empregados();

                    Cargo ValidaCargo = new Cargo();
                    Setor ValidaSetor = new Setor();
                    CentroDeCusto ValidaCentroCusto = new CentroDeCusto();
                    Empregados EmpregadosCadastrados = new Empregados();
                    PesquisaDapperEmpFil dadosEmpresaFilial;


                    int? novoIDcargoInserido = 0;
                    int? novoIDSetorInserido = 0;
                    int? novoIDcentroCustoInserido = 0;
                    int? idEstadoCivil = 0;
                    int? idCategoriaTrabalhador = 0;
                    int? idTipoContratacao = 0;
                    int? idSituacaoEmpregado = 0;
                    int? idNecessidadeEspecial = 0;

                    string conectionString = contexto.RetornaConectionStrings();
                    using (SqlConnection dbDapper = new SqlConnection(conectionString))
                    {
                        dbDapper.Open();
                        string querySelect = "SELECT emp.IdCliente empresa, fil.IdFilial filial FROM Med.Filial fil LEFT JOIN Med.Cliente emp(NOLOCK) ON emp.IdEmpresa = fil.IdEmpresa AND emp.IdCliente = fil.IdCliente WHERE Adm.ufcTrataZeros(REPLACE(REPLACE(REPLACE(fil.CnpjCpf, '.', ''), '/', ''), '-', ''), 14, 0) = Adm.ufcTrataZeros(REPLACE(REPLACE(REPLACE('" + dados.CnpjFilial + "', '.', ''), '/', ''), '-', ''), 14, 0)";
                        dadosEmpresaFilial = dbDapper.Query<PesquisaDapperEmpFil>(querySelect).First();
                        dbDapper.Dispose();
                    }

                    // Valida se cargo existe

                    try
                    {
                        ValidaCargo = ctx.TabelaCargo.Where(r => r.CodigoRH.Trim() == dados.CodCargo.Trim() && r.IdCliente == dadosEmpresaFilial.empresa && r.IdFilial == dadosEmpresaFilial.filial).First();
                    }
                    catch (Exception e)
                    {
                        ValidaCargo = null;
                    }

                    // Valida se Setor existe

                    try
                    {
                        ValidaSetor = ctx.TabelaSetor.Where(r => r.CodigoRH.Trim() == dados.CodSetor.Trim() && r.IdCliente == dadosEmpresaFilial.empresa && r.IdFilial == dadosEmpresaFilial.filial).First();
                    }
                    catch (Exception e)
                    {
                        ValidaSetor = null;
                    }

                    // Valida se Centro de custo existe 

                    try
                    {

                        ValidaCentroCusto = ctx.TabelaCentroCusto.Where(r => r.IdEmpresa == 1 && r.Codigo.Trim() == dados.CodCentroCusto.Trim() && r.IdCliente == dadosEmpresaFilial.empresa && r.IdFilial == dadosEmpresaFilial.filial).First();

                    }
                    catch (Exception e)
                    {
                        ValidaCentroCusto = null;
                    }

                    // Procura empregado cadastrado 

                    try
                    {
                        EmpregadosCadastrados = ctx.TabelaEmpregados.Where(r => r.CnpjCpf.Trim() == dados.CpfRne.Trim() && r.IdCliente == dadosEmpresaFilial.empresa && r.IdFilial == dadosEmpresaFilial.filial)
                                                                            .Select(x => new Empregados() { IdEmpregado = x.IdEmpregado }).First();
                    }
                    catch (Exception e)
                    {
                        EmpregadosCadastrados = null;
                    }

                    // Get Estado civil 

                    try
                    {
                        idEstadoCivil = ctx.TabelaEstadoCivil.Where(r => r.Nome.Trim().Substring(0, 1).Trim() == dados.EstadoCivil.Substring(0, 1).Trim()).First().IdEstadoCivil;
                    }
                    catch (Exception e)
                    {
                        idEstadoCivil = null;
                    }

                    // Get Categoria Trabalhador 
                    try
                    {
                        idCategoriaTrabalhador = ctx.TabelaCategoriaTrabalhador.Where(r => r.Codigo.Trim() == dados.CategTrabalhador.Trim()).First().IdCategoriaTrabalhador;
                    }
                    catch (Exception e)
                    {
                        idCategoriaTrabalhador = null;
                    }

                    // Get id tipo Contratação
                    try
                    {
                        Int16 idTipo16 = ctx.TabelaTipoContratacao.Where(r => r.Codigo.Trim() == (dados.TpContratacao.Length == 1 ? "0" + dados.TpContratacao : dados.TpContratacao)).First().IdTipoContratacao;
                        idTipoContratacao = int.Parse(idTipo16.ToString());
                    }
                    catch (Exception e)
                    {
                        idTipoContratacao = null;
                    }

                    // Get id situação empregado 
                    try
                    {
                        Int16 idSituacao16 = ctx.TabelaSituacaoEmpregado.Where(r => r.Codigo.Trim() == (dados.Situacao.Length == 1 ? "0" + dados.Situacao : dados.Situacao)).First().IdSituacaoEmpregado;
                        idSituacaoEmpregado = int.Parse(idSituacao16.ToString());
                    }
                    catch (Exception e)
                    {
                        idSituacaoEmpregado = null;
                    }

                    // Get Necessidade especial

                    try
                    {
                        idNecessidadeEspecial = ctx.TabelaNecessidadeEspecial.Where(r => r.Codigo.Trim() == (dados.NecessidadeEspecial.Trim().Length == 1 ? "0" + dados.NecessidadeEspecial.Trim() : dados.NecessidadeEspecial.Trim())).First().IdTipoNecessidadeEspecial;
                    }
                    catch (Exception e)
                    {
                        idNecessidadeEspecial = null;
                    }


                    // Se não existir Cargo insere registro

                    if (ValidaCargo == null)
                    {
                        try
                        {
                            Cargo NovoCargo = new Cargo();

                            NovoCargo.IdEmpresa = 1;
                            NovoCargo.IdCliente = dadosEmpresaFilial.empresa;
                            NovoCargo.IdFilial = dadosEmpresaFilial.filial;
                            NovoCargo.Codigo = dados.CodCargo;
                            NovoCargo.CodigoRH = dados.CodCargo;
                            NovoCargo.CodigoCBO = dados.NumCBO;
                            NovoCargo.Nome = dados.NomeCargo;
                            NovoCargo.DescricaoDetalhada = dados.DescrAtividade;
                            NovoCargo.Ativo = true;

                            ctx.TabelaCargo.Add(NovoCargo);
                            ctx.SaveChanges();
                            novoIDcargoInserido = NovoCargo.IdCargo;
                            cargosInseridos.Add(NovoCargo);
                            QtdCargosInseridos++;

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Erro no cadastro de Cargo " + e.InnerException);
                            
                        }
                       

                    }

                    // Se não existir setor insere registro

                    if (ValidaSetor == null)
                    {

                        try {
                            Setor NovoSetor = new Setor();
                            NovoSetor.IdEmpresa = 1;
                            NovoSetor.IdCliente = dadosEmpresaFilial.empresa;
                            NovoSetor.IdFilial = dadosEmpresaFilial.filial;
                            NovoSetor.Codigo = dados.CodSetor;
                            NovoSetor.CodigoRH = dados.CodSetor;
                            NovoSetor.Nome = dados.NomeSetor;
                            NovoSetor.Ativo = true;

                            ctx.TabelaSetor.Add(NovoSetor);
                            ctx.SaveChanges();
                            novoIDSetorInserido = NovoSetor.IdSetor;
                            SetoresInseridos.Add(NovoSetor);
                            QtdSetorInseridos++;
                        }catch(Exception e)
                        {
                            Console.WriteLine("Erro no cadastro de Setor " + e.InnerException);
                            
                        }

                    }

                    // Se não existir centro de custo insere registro

                    if (ValidaCentroCusto == null && (dados.CodCentroCusto.Trim() != "" && dados.CodCentroCusto.Trim() != null) && (dados.NomeCentroCusto.Trim() != "" && dados.NomeCentroCusto.Trim() != null))
                    {
                        try
                        {
                            CentroDeCusto NovoCentroCusto = new CentroDeCusto();
                            NovoCentroCusto.IdEmpresa = 1;
                            NovoCentroCusto.IdCliente = dadosEmpresaFilial.empresa;
                            NovoCentroCusto.IdFilial = dadosEmpresaFilial.filial;
                            NovoCentroCusto.Codigo = dados.CodCentroCusto.Trim();
                            NovoCentroCusto.Nome = dados.NomeCentroCusto.Trim();
                            NovoCentroCusto.Tipo = "INTERFACE";
                            NovoCentroCusto.Ativo = true;


                            ctx.TabelaCentroCusto.Add(NovoCentroCusto);
                            ctx.SaveChanges();
                            novoIDcentroCustoInserido = NovoCentroCusto.IdCentroCusto;
                            CentroCustoInseridos.Add(NovoCentroCusto);
                            QtdCentroCustoInseridos++;


                        }
                        catch(Exception e)
                        {
                            Console.WriteLine("Erro no cadastro de Centro de Custo " + e.InnerException);
                           
                        }
                        

                    }

                    try
                    {
                        // Cadastra um novo Empregado
                        if (EmpregadosCadastrados == null)
                        {
                            Empregados novoEmpregado = new Empregados();
                            novoEmpregado.IdEmpresa = 1;
                            novoEmpregado.IdCliente = dadosEmpresaFilial.empresa;
                            novoEmpregado.IdFilial = dadosEmpresaFilial.filial;
                            // Valida se foi inserido um novo cargo ou coloca o que ja existe
                            novoEmpregado.IdCargo = ValidaCargo == null ? novoIDcargoInserido : ValidaCargo.IdCargo;
                            // Valida se foi inserido um novo Setor ou coloca o que ja existe
                            novoEmpregado.IdSetor = ValidaSetor == null ? novoIDSetorInserido : ValidaSetor.IdSetor;
                            // Valida se foi inserido um novo centro de custo ou coloca o que ja existe 
                            novoEmpregado.IdCentroCusto = ValidaCentroCusto == null ? ((dados.CodCentroCusto.Trim() != "" && dados.CodCentroCusto.Trim() != null) && (dados.NomeCentroCusto.Trim() != "" && dados.NomeCentroCusto.Trim() != null) ? novoIDcentroCustoInserido : null) : ValidaCentroCusto.IdCentroCusto;
                            novoEmpregado.IdEstadoCivil = idEstadoCivil;
                            novoEmpregado.IdEnderecoPrincipal = null;
                            novoEmpregado.IdContatoPrincipal = null;
                            novoEmpregado.IdContaPrincipal = null;
                            novoEmpregado.IdCategoriaTrabalhador = idCategoriaTrabalhador;
                            novoEmpregado.IdTipoContratacao = (idTipoContratacao != null ? idTipoContratacao : null);
                            novoEmpregado.IdSituacaoEmpregado = (idSituacaoEmpregado != null ? (Int32?)idSituacaoEmpregado : (Int32?)null);
                            novoEmpregado.IdTipoNecessidadeEspecial = (idNecessidadeEspecial != null ? (Int32?)idNecessidadeEspecial : (Int32?)null);
                            novoEmpregado.NumeroChapa = dados.NumChapa;
                            novoEmpregado.Matricula = dados.NumMatricula;
                            novoEmpregado.Nome = dados.NomeFuncionario;
                            try
                            {
                                if (dados.DtNascimento == DateTime.MinValue)
                                {
                                    novoEmpregado.DataNascimento = null;
                                }
                                else
                                {
                                    novoEmpregado.DataNascimento = dados.DtNascimento;
                                }

                            }
                            catch
                            {
                                novoEmpregado.DataNascimento = null;
                            }
                            novoEmpregado.TipoPessoa = "F";
                            novoEmpregado.Sexo = dados.Sexo;
                            novoEmpregado.CnpjCpf = dados.CpfRne;
                            novoEmpregado.RgUF = dados.RgUF;
                            novoEmpregado.Rg = dados.RgNumero;
                            novoEmpregado.RgOrgao = dados.RgOrgaoEmissor;
                            try
                            {
                                if (dados.RgDtEmissao == DateTime.MinValue)
                                {
                                    novoEmpregado.RgDataEmissao = null;
                                }
                                else
                                {
                                    novoEmpregado.RgDataEmissao = dados.RgDtEmissao;
                                }

                            }
                            catch
                            {
                                novoEmpregado.RgDataEmissao = null;
                            }

                            novoEmpregado.CtpsUF = dados.CtpsUf;
                            novoEmpregado.Ctps = dados.CtpsNumero;
                            novoEmpregado.CtpsSerie = dados.CtpsNumero;
                            try
                            {
                                if (dados.CtpsDtEmissao == DateTime.MinValue)
                                {
                                    novoEmpregado.CtpsDataEmissao = null;
                                }
                                else
                                {
                                    novoEmpregado.CtpsDataEmissao = dados.CtpsDtEmissao;
                                }

                            }
                            catch
                            {
                                novoEmpregado.CtpsDataEmissao = null;
                            }
                            novoEmpregado.Pis = dados.NumPisNit;
                            novoEmpregado.Gfip = dados.NumGfip;
                            novoEmpregado.Ativo = true;
                            novoEmpregado.Observacao = "IMPORTADO EM: " + DateTime.Now.ToShortDateString() + " - CHAVE:  " + dados.CdChaveAcao + "LINHA PLANILHA :" + dados.NumLinha;
                            try
                            {
                                novoEmpregado.DataAdmissao = dados.DtAdmissao;
                            }
                            catch
                            {
                                novoEmpregado.DataAdmissao = null;
                            }

                            try
                            {
                                if (dados.DtDemissao == DateTime.MinValue)
                                {
                                    novoEmpregado.DataDemissao = null;
                                }
                                else
                                {
                                    novoEmpregado.DataDemissao = dados.DtDemissao;

                                }
                            }
                            catch
                            {
                                novoEmpregado.DataDemissao = null;
                            }
                            novoEmpregado.Periculosidade = (dados.AdicPericulosidade.ToUpper().Trim() == "S" ? true : false);
                            novoEmpregado.Telefone = dados.Telefone.Trim();
                            novoEmpregado.Insalubridade = (dados.AdicInsalubridade.ToUpper().Trim() == "S" ? true : false);
                            novoEmpregado.GrauInsalubridade = (dados.AdicInsalubridade.ToUpper().Trim() == "S" ? dados.GrauInsalubridade : null);
                            novoEmpregado.GrauPericulosidade = (dados.AdicPericulosidade.ToUpper().Trim() == "S" ? dados.GrauPericulosidade : null);
                            novoEmpregado.TurnoTrabalho = dados.DescrTurnoTrabalho.Trim();
                            novoEmpregado.IdUsuarioInclusao = 999;
                            novoEmpregado.DataUsuarioInclusao = DateTime.Now;
                            novoEmpregado.IdUsuarioAlteracao = null;
                            novoEmpregado.DataUsuarioAlteracao = null;
                            // Adiciona uma linha nova de empregado
                            objetoAtual = novoEmpregado;
                            ctx.TabelaEmpregados.Add(novoEmpregado);
                            ctx.SaveChanges();

                            // Caso tenha um dependente , vincula o id do empregado ao dependente
                            if (novoEmpregado.IdEmpregado != null && (dados.NomeDependente.Trim() != "" && dados.NomeDependente.Trim() != null))
                            {
                                string codigoTipoDependente = "";
                                if (dados.TpDependente.Length == 1)
                                {
                                    codigoTipoDependente = "0" + dados.TpDependente;
                                }
                                else
                                {
                                    codigoTipoDependente = dados.TpDependente;
                                }

                                int? idDependente = 0;
                                try
                                {
                                    idDependente = ctx.TabelaTipoDependente.Where(f => f.Codigo.Trim() == codigoTipoDependente).First().IdTipoDependente;
                                }
                                catch (Exception e)
                                {
                                    idDependente = null;
                                }

                                Dependente novoDependente = new Dependente();
                                novoDependente.IdEmpresa = 1;
                                novoDependente.IdEmpregado = novoEmpregado.IdEmpregado;
                                novoDependente.IdTipoDependente = idDependente;
                                novoDependente.Nome = dados.NomeDependente.Trim();
                                novoDependente.CnpjCpf = dados.CpfDependente.Trim();
                                novoDependente.IdGrauParentesco = null;
                                novoDependente.IdUsuarioInclusao = 999;
                                novoDependente.DataInclusao = DateTime.Now;
                                ctx.TabelaDependente.Add(novoDependente);
                                ctx.SaveChanges();


                            }

                            // Se o empregado foi gravado , gera o endereço na tabela de endereços

                            if (novoEmpregado.IdEmpregado != 0)
                            {
                                Int32? idTipoEndereco32 = 0;
                                Int32? idTipoLogradouroInt32 = 0;
                                Int16 idTipoEndereco16 = ctx.TabelaTpEndereco.Where(f => f.Codigo.Trim().ToUpper() == "RES").First().IdTipoEndereco;
                                string descricaoTipoLogradouro = "";

                                try
                                {

                                    idTipoEndereco32 = Int32.Parse(idTipoEndereco16.ToString());

                                }
                                catch (Exception e)
                                {
                                    idTipoEndereco32 = null;
                                }

                                try
                                {
                                    Int16 idTipoLogradouroInt = ctx.TabelaTpLogradouro.Where(r => r.Codigo.Trim() == dados.TpLogradouro.Trim().Substring(0, 1)).First().IdTipoLogradouro;
                                    idTipoLogradouroInt32 = Int32.Parse(idTipoLogradouroInt.ToString());
                                }
                                catch (Exception e)
                                {
                                    idTipoLogradouroInt32 = null;
                                }

                                try
                                {
                                    descricaoTipoLogradouro = ctx.TabelaTpLogradouro.Where(r => r.IdTipoLogradouro == idTipoLogradouroInt32).First().Nome;
                                }
                                catch
                                {
                                    descricaoTipoLogradouro = "RUA";
                                }

                                EnderecoEmpregado novoEndereco = new EnderecoEmpregado();

                                novoEndereco.IdEmpresa = 1;
                                novoEndereco.IdEmpregado = novoEmpregado.IdEmpregado;
                                novoEndereco.IdTipoEndereco = (idTipoEndereco32 == null ? 5 : idTipoEndereco32);
                                novoEndereco.IdTipoLogradouro = (idTipoLogradouroInt32 == null ? 33 : idTipoLogradouroInt32);
                                novoEndereco.TipoLogradouro = descricaoTipoLogradouro;
                                novoEndereco.Logradouro = dados.Logradouro;
                                novoEndereco.Numero = dados.Numero;
                                novoEndereco.Complemento = dados.Complemento;
                                novoEndereco.Bairro = dados.Bairro;
                                novoEndereco.Cidade = dados.Cidade;
                                novoEndereco.Uf = dados.UF;
                                novoEndereco.Pais = dados.Pais;
                                novoEndereco.Cep = dados.CEP;
                                novoEndereco.Observacao = "IMPORTADO EM " + DateTime.Now.ToShortDateString();
                                novoEndereco.Principal = true;
                                novoEndereco.IdUsuarioInclusao = 999;
                                novoEndereco.DataUsuarioInclusao = DateTime.Now;

                                ctx.TabelaEnderecoEmpregado.Add(novoEndereco);
                                ctx.SaveChanges();


                                // Vincula dados do endereco do empregado no cadastro do empregado
                                Empregados VinculaEnderecoEmpregado = ctx.TabelaEmpregados.Where(f => f.IdEmpregado == novoEmpregado.IdEmpregado).First();
                                VinculaEnderecoEmpregado.IdEnderecoPrincipal = (Int64?)novoEndereco.IdEnderecoEmpregado;
                                ctx.TabelaEmpregados.Update(VinculaEnderecoEmpregado);
                                ctx.SaveChanges();


                            }


                            EmpregadosInseridos.Add(novoEmpregado);

                            QtdEmpregadosInseridos++;

                        }

                        // Atualiza Empregado
                        else
                        {
                            Empregados DadoUpdate = ctx.TabelaEmpregados.Where(r => r.IdEmpregado == EmpregadosCadastrados.IdEmpregado).First();

                            DadoUpdate.IdEmpresa = 1;
                            DadoUpdate.IdCliente = dadosEmpresaFilial.empresa;
                            DadoUpdate.IdFilial = dadosEmpresaFilial.filial;
                            // Valida se foi inserido um novo cargo ou coloca o que ja existe
                            DadoUpdate.IdCargo = ValidaCargo == null ? novoIDcargoInserido : ValidaCargo.IdCargo;
                            // Valida se foi inserido um novo Setor ou coloca o que ja existe
                            DadoUpdate.IdSetor = ValidaSetor == null ? novoIDSetorInserido : ValidaSetor.IdSetor;
                            // Valida se foi inserido um novo centro de custo ou coloca o que ja existe 
                            DadoUpdate.IdCentroCusto = ValidaCentroCusto == null ? ((dados.CodCentroCusto.Trim() != "" && dados.CodCentroCusto.Trim() != null) && (dados.NomeCentroCusto.Trim() != "" && dados.NomeCentroCusto.Trim() != null) ? novoIDcentroCustoInserido : null) : ValidaCentroCusto.IdCentroCusto;
                            DadoUpdate.IdEstadoCivil = idEstadoCivil;
                            DadoUpdate.IdEnderecoPrincipal = DadoUpdate.IdEnderecoPrincipal;
                            DadoUpdate.IdContatoPrincipal = null;
                            DadoUpdate.IdContaPrincipal = null;
                            DadoUpdate.IdCategoriaTrabalhador = idCategoriaTrabalhador;
                            DadoUpdate.IdTipoContratacao = idTipoContratacao;
                            DadoUpdate.IdSituacaoEmpregado = idSituacaoEmpregado;
                            DadoUpdate.IdTipoNecessidadeEspecial = idNecessidadeEspecial;
                            DadoUpdate.NumeroChapa = dados.NumChapa;
                            DadoUpdate.Matricula = dados.NumMatricula;
                            DadoUpdate.Nome = dados.NomeFuncionario;
                            try
                            {
                                DadoUpdate.DataNascimento = dados.DtNascimento;
                            }
                            catch
                            {
                                DadoUpdate.DataNascimento = null;
                            }

                            DadoUpdate.TipoPessoa = "F";
                            DadoUpdate.Sexo = dados.Sexo;
                            DadoUpdate.CnpjCpf = dados.CpfRne;
                            DadoUpdate.RgUF = dados.RgUF;
                            DadoUpdate.Rg = dados.RgNumero;
                            DadoUpdate.RgOrgao = dados.RgOrgaoEmissor;
                            try
                            {
                                DadoUpdate.RgDataEmissao = dados.RgDtEmissao;
                            }
                            catch
                            {
                                DadoUpdate.RgDataEmissao = null;
                            }

                            DadoUpdate.CtpsUF = dados.CtpsUf;
                            DadoUpdate.Ctps = dados.CtpsNumero;
                            DadoUpdate.CtpsSerie = dados.CtpsNumero;
                            try
                            {
                                DadoUpdate.CtpsDataEmissao = dados.CtpsDtEmissao;
                            }
                            catch
                            {
                                DadoUpdate.CtpsDataEmissao = null;
                            }

                            DadoUpdate.Pis = dados.NumPisNit;
                            DadoUpdate.Ativo = true;
                            DadoUpdate.Gfip = dados.NumGfip;
                            DadoUpdate.Observacao = "ATUALIZADO EM: " + DateTime.Now.ToShortDateString() + " - CHAVE:  " + dados.CdChaveAcao + "  LINHA PLANILHA :" + dados.NumLinha;
                            try
                            {
                                if (dados.DtAdmissao == DateTime.MinValue)
                                {
                                    DadoUpdate.DataAdmissao = null;
                                }
                                else
                                {
                                    DadoUpdate.DataAdmissao = dados.DtAdmissao;
                                }
                            }
                            catch
                            {
                                DadoUpdate.DataAdmissao = null;
                            }

                            try
                            {
                                if (dados.DtDemissao == DateTime.MinValue)
                                {
                                    DadoUpdate.DataDemissao = null;
                                }
                                else
                                {
                                    DadoUpdate.DataDemissao = dados.DtDemissao;
                                }

                            }
                            catch
                            {
                                DadoUpdate.DataDemissao = null;
                            }

                            DadoUpdate.Periculosidade = (dados.AdicPericulosidade.ToUpper().Trim() == "S" ? true : false);
                            DadoUpdate.Telefone = dados.Telefone.Trim();
                            DadoUpdate.Insalubridade = (dados.AdicInsalubridade.ToUpper().Trim() == "S" ? true : false);
                            DadoUpdate.GrauInsalubridade = (dados.AdicInsalubridade.ToUpper().Trim() == "S" ? dados.GrauInsalubridade : null);
                            DadoUpdate.GrauPericulosidade = (dados.AdicPericulosidade.ToUpper().Trim() == "S" ? dados.GrauPericulosidade : null);
                            DadoUpdate.TurnoTrabalho = dados.DescrTurnoTrabalho.Trim();
                            DadoUpdate.IdUsuarioInclusao = DadoUpdate.IdUsuarioInclusao;
                            DadoUpdate.DataUsuarioInclusao = DadoUpdate.DataUsuarioInclusao;
                            DadoUpdate.IdUsuarioAlteracao = 999;
                            DadoUpdate.DataUsuarioAlteracao = DateTime.Now;
                            objetoAtual = DadoUpdate;
                            // Atualiza um empregado ja criado
                            ctx.TabelaEmpregados.Update(DadoUpdate);
                            ctx.SaveChanges();



                            // Update dados endereço 


                            Int32? idTipoEndereco32 = 0;
                            Int32? idTipoLogradouroInt32 = 0;
                            Int16 idTipoEndereco16 = ctx.TabelaTpEndereco.Where(f => f.Codigo.Trim().ToUpper() == "RES").First().IdTipoEndereco;

                            try
                            {

                                idTipoEndereco32 = Int32.Parse(idTipoEndereco16.ToString());

                            }
                            catch (Exception e)
                            {
                                idTipoEndereco32 = null;
                            }

                            try
                            {
                                Int16 idTipoLogradouroInt = ctx.TabelaTpLogradouro.Where(r => r.Codigo.Trim() == dados.TpLogradouro.Trim().Substring(0,1)).First().IdTipoLogradouro;
                                idTipoLogradouroInt32 = Int32.Parse(idTipoLogradouroInt.ToString());
                            }
                            catch (Exception e)
                            {
                                idTipoLogradouroInt32 = null;
                            }


                            EnderecoEmpregado enderecoUpdate = ctx.TabelaEnderecoEmpregado.Where(r => r.IdEmpregado == DadoUpdate.IdEmpregado).First();

                            enderecoUpdate.IdEmpresa = 1;
                            enderecoUpdate.IdEmpregado = DadoUpdate.IdEmpregado;
                            enderecoUpdate.IdTipoEndereco = (idTipoEndereco32 == null ? 5 : idTipoEndereco32);
                            enderecoUpdate.IdTipoLogradouro = (idTipoLogradouroInt32 == null ? 33 : idTipoLogradouroInt32);
                            enderecoUpdate.Logradouro = dados.Logradouro;
                            enderecoUpdate.Numero = dados.Numero;
                            enderecoUpdate.Complemento = dados.Complemento;
                            enderecoUpdate.Bairro = dados.Bairro;
                            enderecoUpdate.Cidade = dados.Cidade;
                            enderecoUpdate.Uf = dados.UF;
                            enderecoUpdate.Pais = dados.Pais;
                            enderecoUpdate.Cep = dados.CEP;
                            enderecoUpdate.Observacao = "ATUALIZADO EM " + DateTime.Now.ToShortDateString();
                            enderecoUpdate.Principal = true;
                            enderecoUpdate.IdUsuarioInclusao = enderecoUpdate.IdUsuarioInclusao;
                            enderecoUpdate.DataUsuarioInclusao = enderecoUpdate.DataUsuarioInclusao;
                            enderecoUpdate.IdUsuarioAlteracao = 999;
                            enderecoUpdate.DataUsuarioAlteracao = DateTime.Now;
                            ctx.TabelaEnderecoEmpregado.Update(enderecoUpdate);

                            ctx.SaveChanges();



                            // Atualiza dependente


                            // Caso tenha um dependente , vincula o id do empregado ao dependente
                            if (DadoUpdate.IdEmpregado != null && (dados.NomeDependente.Trim() != "" && dados.NomeDependente.Trim() != null))
                            {
                                string codigoTipoDependente = "";
                                if (dados.TpDependente.Length == 1)
                                {
                                    codigoTipoDependente = "0" + dados.TpDependente;
                                }
                                else
                                {
                                    codigoTipoDependente = dados.TpDependente;
                                }

                                int? idDependente = 0;
                                try
                                {
                                    idDependente = ctx.TabelaTipoDependente.Where(f => f.Codigo.Trim() == codigoTipoDependente).First().IdTipoDependente;
                                }
                                catch (Exception e)
                                {
                                    idDependente = null;
                                }

                                Dependente AtualizaDependente = ctx.TabelaDependente.Where(r => r.IdEmpregado == DadoUpdate.IdEmpregado).First();
                                AtualizaDependente.IdEmpresa = 1;
                                AtualizaDependente.IdEmpregado = AtualizaDependente.IdEmpregado;
                                AtualizaDependente.IdTipoDependente = idDependente;
                                AtualizaDependente.Nome = dados.NomeDependente.Trim();
                                AtualizaDependente.CnpjCpf = dados.CpfDependente.Trim();
                                AtualizaDependente.IdGrauParentesco = null;
                                AtualizaDependente.DataInclusao = AtualizaDependente.DataInclusao;
                                AtualizaDependente.IdUsuarioInclusao = AtualizaDependente.IdUsuarioInclusao;
                                AtualizaDependente.DataAlteracao = DateTime.Now;
                                AtualizaDependente.IdUsuarioAlteracao = 999;
                                ctx.TabelaDependente.Update(AtualizaDependente);
                                ctx.SaveChanges();


                            }

                            EmpregadosAtualizados.Add(DadoUpdate);
                            QtdEmpregadosAtualizados++;

                        }

                    }
                    catch (Exception e)
                    {
                        errosEmpregados.Add(new KeyValuePair<Empregados, string>(objetoAtual, "Erro na inserção do dado , detalhes do erro \n " + e.Message + "\n \n " + e.InnerException + "\n \n "));
                    }

                }
            }

            retornoDadosGravacao.ErrosEmpregados = errosEmpregados;
            retornoDadosGravacao.QtdCargoUpload = new KeyValuePair<List<Cargo>, int>(cargosInseridos, QtdCargosInseridos);
            retornoDadosGravacao.QtdCentroCustoUpload = new KeyValuePair<List<CentroDeCusto>, int>(CentroCustoInseridos, QtdCentroCustoInseridos);
            retornoDadosGravacao.QtdSetorUpload = new KeyValuePair<List<Setor>, int>(SetoresInseridos, QtdSetorInseridos);
            retornoDadosGravacao.QtdEmpregadosInseridos = new KeyValuePair<List<Empregados>, int>(EmpregadosInseridos, QtdEmpregadosInseridos);
            retornoDadosGravacao.QtdEmpregadosAtualizados = new KeyValuePair<List<Empregados>, int>(EmpregadosAtualizados, QtdEmpregadosAtualizados);

            return retornoDadosGravacao;

        }



    }
}
