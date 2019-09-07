using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Upload_GeralV1.Models;

namespace Upload_GeralV1
{
    class Program
    {
        static void Main(string[] args)
        {
            var PathProjeto = Directory.GetCurrentDirectory();
            LeitorExcel leitor = new LeitorExcel();
            CargasGeralClass ContextoCargas = new CargasGeralClass();
            // Retira o caminho incluido no Path
            var caminhoReplace = PathProjeto.Replace(@"\bin\Debug\netcoreapp2.2", "");
            // Cria o diretorio de Funcionario se nao existir
            string diretorioEmpregadoMontado = caminhoReplace + @"\ArquivosUpload\CargaFuncionariosExcel\";
            if (!Directory.Exists(diretorioEmpregadoMontado))
            {
                Directory.CreateDirectory(diretorioEmpregadoMontado);
            }

            //Variavel de escolha do usuario
            int EscolhaNum = 0;
            //Variavel de teste para parse
            int testeParse = 0;

        comecoPrograma:

            //Titulo Programa
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(Figgle.FiggleFonts.Standard.Render(" CARGAS TAP     V 1   "));

            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine("    Atenção antes de efetuar a escolha , verificar nas pastas 'CargaFuncionariosExcel' \n    e 'CargaFiliaisExcel' se há  arquivos \n \n  ");
            Console.ResetColor();

            Console.WriteLine("    Escolha a carga/upload que deseja efetuar: \n\n ");
            Console.WriteLine("    1 - Carga de Funcionarios \n\n ");
            Console.WriteLine("    2 - Carga de Filiais \n\n ");
            Console.WriteLine("    3 - Testar conexão com banco \n\n");
            Console.WriteLine("    4 - Zerar mensagens \n\n");
            Console.WriteLine("    0 - Sair \n\n ");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("             ======================================  \n " +
                              "  " +
                              "    Created by: Pedro Vinicius - \u00a9 Sistema TAP V2 - Versão carga: 1.0 \n" +
                              "  " +
                              "           ======================================  \n ");
            Console.ResetColor();


            var dado = Console.ReadLine();
            var testeInteiro = int.TryParse(dado, out testeParse);

            if (testeInteiro == false)
            {
                Console.Clear();
                Console.WriteLine("O dado incluido não corresponde a um numero inteiro , favor incluir novamente \n \n ");
                goto comecoPrograma;
            }
            else
            {
                EscolhaNum = int.Parse(dado);
                if (EscolhaNum != 1 && EscolhaNum != 2 && EscolhaNum != 3 && EscolhaNum != 4 && EscolhaNum != 0)
                {
                    Console.Clear();
                    Console.WriteLine("Não há opção para este numero , Favor escolher novamente ! \n \n");
                    goto comecoPrograma;
                }
            }

            //Começo das escolhas
            switch (EscolhaNum)
            {
                //Faz carga de funcionario
                case 1:
                    Console.Clear();
                    // Entra no diretorio e faz a leitura um por um 
                    string[] arquivosExcelEmpregados = Directory.GetFiles(diretorioEmpregadoMontado);

                    if (arquivosExcelEmpregados.Length > 0)
                    {
                        foreach (string arquivo in arquivosExcelEmpregados)
                        {
                            FileInfo dadosArquivo = new FileInfo(arquivo);
                            string extencao = dadosArquivo.Extension;
                            if (extencao == ".csv")
                            {
                                try
                                {
                                    Console.WriteLine(" \n \n  ------------------------------------------------------  ARQUIVO  " + dadosArquivo.Name.Replace(".csv", "") + "  -----------------------------------------------------------------------  \n \n  ");

                                    Console.WriteLine(" \n \n Montando arquivo '" + dadosArquivo.Name.Replace(".csv", "") + "'... \n \n");
                                    DataTable dados = leitor.MontaExcelCsv(arquivo);

                                    // Recebe um keyValuePair Valor da equerda sao dados corretos prontos para insert 
                                    // valor da direita sao dados para colocar no log
                                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                                    Console.WriteLine(" \n \n Validando arquivo '" + dadosArquivo.Name.Replace(".csv", "") + "' \n \n Por gentileza , aguardar alguns minutos , este processo pode demorar .. \n \n ");
                                    Console.ResetColor();
                                    KeyValuePair<List<ITFEmpregado>, List<ITFEmpregado>> retornoValidacao = new KeyValuePair<List<ITFEmpregado>, List<ITFEmpregado>>();
                                    retornoValidacao = ContextoCargas.ValidaArquivoUploadEmpregado(dadosArquivo.Name.Replace(".csv", ""), dados);
                                    if (retornoValidacao.Key.Count > 0)
                                    {
                                        // Faz insert de todos os dados corretos
                                        Console.WriteLine("\n \n  GRAVANDO DADOS CORRETOS  ! \n \n ");

                                        RetornoCargaFuncionarios retornoValidacaoGravacao = ContextoCargas.GravaDadosEmpregados(retornoValidacao.Key);

                                        Console.WriteLine(" \n \n  ---------------------------------------- DETALHES DA CARGA  -------------------------------------------------- \n \n ");

                                        if (retornoValidacaoGravacao.QtdCargoUpload.Value != 0)
                                        {
                                            Console.WriteLine("CARGOS INSERIDOS : \n \n ");

                                            foreach (Cargo cargosInseridos in retornoValidacaoGravacao.QtdCargoUpload.Key)
                                            {
                                                Console.WriteLine(" * Nome Cargo : " + cargosInseridos.Nome + "\n" + "* ID cargo : " + cargosInseridos.IdCargo + "\n \n ");

                                            }
                                        }

                                        if (retornoValidacaoGravacao.QtdCentroCustoUpload.Value != 0)
                                        {
                                            Console.WriteLine("CENTRO DE CUSTOS INSERIDOS : \n \n ");
                                            foreach (CentroDeCusto centrodeCustos in retornoValidacaoGravacao.QtdCentroCustoUpload.Key)
                                            {
                                                Console.WriteLine(" * Nome Centro de custo : " + centrodeCustos.Nome + "\n" + "* ID Centro de custo : " + centrodeCustos.IdCentroCusto + "\n \n ");
                                            }
                                        }

                                        if (retornoValidacaoGravacao.QtdSetorUpload.Value != 0)
                                        {
                                            Console.WriteLine("SETORES  INSERIDOS : \n \n ");
                                            foreach (Setor setores in retornoValidacaoGravacao.QtdSetorUpload.Key)
                                            {
                                                Console.WriteLine(" * Nome Setor : " + setores.Nome + "\n" + "* ID Setor : " + setores.IdSetor + "\n \n ");
                                            }
                                        }

                                        if (retornoValidacaoGravacao.QtdEmpregadosInseridos.Value != 0)
                                        {
                                            Console.WriteLine("EMPREGADOS  INSERIDOS : \n \n ");
                                            foreach (Empregados empregadosinseridos in retornoValidacaoGravacao.QtdEmpregadosInseridos.Key)
                                            {
                                                Console.WriteLine(" * Nome Empregado : " + empregadosinseridos.Nome + "\n" + "* ID Empregado : " + empregadosinseridos.IdEmpregado + "\n \n ");
                                            }

                                        }

                                        if (retornoValidacaoGravacao.QtdEmpregadosAtualizados.Value != 0)
                                        {
                                            Console.WriteLine("EMPREGADOS  ATUALIZADOS : \n \n ");
                                            foreach (Empregados empregadosatualizados in retornoValidacaoGravacao.QtdEmpregadosAtualizados.Key)
                                            {
                                                Console.WriteLine(" * Nome Empregado: " + empregadosatualizados.Nome + "\n" + "* ID Empregado : " + empregadosatualizados.IdEmpregado + "\n \n ");
                                            }
                                        }

                                        if (retornoValidacaoGravacao.ErrosEmpregados.Count() > 0)
                                        {
                                            Console.WriteLine("ERROS INSERÇÃO DE DADOS EMPREGADOS : \n \n ");
                                            foreach (KeyValuePair<Empregados, string> validacoes in retornoValidacaoGravacao.ErrosEmpregados)
                                            {
                                                Console.WriteLine("Empregado " + validacoes.Key.Nome + " " + " Id Empregado " + validacoes.Key.IdEmpregado + "\n \n ");
                                            }
                                        }

                                        Console.WriteLine(" \n \n  -----------------------------------------------------------------------------------------------------------------------------  \n \n  ");

                                        Console.WriteLine("Montando log de inserção de dados , por gentileza , aguardar alguns minutos , este processo pode demorar um pouco ... \n \n ");

                                        bool retorno = ContextoCargas.montaLogInsertDadosEmpregados(dadosArquivo.Name.Replace(".csv", ""), retornoValidacaoGravacao, caminhoReplace);
                                        if (retorno == true)
                                        {
                                            Console.WriteLine("\n \n  CARGA FUNCIONARIO EFETUADA COM SUCESSO ! \n \n ");
                                        }
                                        else
                                        {
                                            Console.WriteLine("\n \n  CARGA FUNCIONARIO EFETUADA COM SUCESSO , LOG NÃO FOI GERADO ! \n \n ");
                                        }


                                    }

                                    if (retornoValidacao.Value.Count > 0)
                                    {
                                        // Monta log de todos os registros que deram erro
                                        var retornoGeraLog = ContextoCargas.montaLogRegistro(dadosArquivo.Name.Replace(".csv", ""), retornoValidacao.Value, caminhoReplace);
                                        if (retornoGeraLog == true)
                                        {
                                            try
                                            {
                                                // Grava dados na transitoria
                                                Console.ForegroundColor = ConsoleColor.Red;
                                                Console.WriteLine(" \n\n Houve algumas inconsistencias com o arquivo '" + dadosArquivo.Name.Replace(".csv", "") + "' para mais detalhes entrar no caminho " + caminhoReplace + @"\Logs_de_erros\" + "\n\n ");
                                                Console.WriteLine(" \n\n Total inconsistencias: " + retornoValidacao.Value.Count + " \n \n ");
                                                Console.ResetColor();

                                                Console.WriteLine("\n \n Gravando dados na transitoria  \n \n");
                                                ContextoCargas.gravaDadosTransitoria(retornoValidacao.Value.Where(r => r.MsgCritica != "").ToList());
                                                Console.ForegroundColor = ConsoleColor.Green;
                                                Console.WriteLine(" \n \n Dados do arquivo '" + dadosArquivo.Name + "' gravados com sucesso na Transitoria ! \n \n ");
                                                Console.ResetColor();

                                            }
                                            catch (Exception e)
                                            {
                                                Console.WriteLine("Houve um erro ao gravar os dados do arquivo '" + dadosArquivo.Name.Replace(".csv", "") + "' na tabela transitoria , segue mensagem de erro: \n\n " + e.Source + "\n \n" + e.Message + "\n \n " + e.InnerException + "\n \n ");
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("Erro na gravação do log do arquivo '" + dadosArquivo.Name + "' ! Dados nao foram salvos na transitoria  !");
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Houve um erro na leitura do arquivo ! '" + dadosArquivo.Name + "' \n \n  Segue detalhes dos erros : \n \n " + e.Message + "\n \n " + e.InnerException +"\n \n");
                                    Console.ResetColor();
                                }
                            }
                            else
                            {
                                Console.WriteLine("O arquivo '" + dadosArquivo.Name + "' Não foi importado pois não é extenção CSV \n \n ");
                                File.Delete(arquivo);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine(" \n \n Não há arquivos na pasta ! Favor incluir algum arquivo csv na pasta para leitura ! \n \n");
                        goto comecoPrograma;
                    }

                    foreach (string arquivo in arquivosExcelEmpregados)
                    {
                        File.Delete(arquivo);
                    }

                    Console.WriteLine("Os dados da pasta Empregados foram zerados !");

                    goto comecoPrograma;


                //Faz carga de Filiais 
                case 2:
                    Console.Clear();
                    //Console.WriteLine("\n \n CARGA FILIAL EFETUADA COM SUCESSO ! \n \n ");
                    Console.WriteLine("\n \n * Em Desenvolvimento * \n \n ");
                    goto comecoPrograma;


                case 3:
                    Console.Clear();
                    // Function para testar conexao com o banco de dados

                    var retornoTeste = ContextoCargas.testaConexaoBD();

                    if (retornoTeste.Key == false && retornoTeste.Value == "")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(" \n \n Falha ao conectar com o banco ! \n \n ");
                    }
                    else if (retornoTeste.Key == false && retornoTeste.Value != "")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(" \n \n Falha ao conectar com o banco ! Segue detalhes do erro abaixo : \n \n " + retornoTeste.Value.ToString() + "\n \n");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(" \n Tudo ok ! \n \n");
                    }

                    Console.ResetColor();
                    goto comecoPrograma;



                case 4:
                    Console.Clear();
                    goto comecoPrograma;



                //Sai da aplicação
                case 0:
                    Environment.Exit(1);
                    break;


            }

        }
    }
}
