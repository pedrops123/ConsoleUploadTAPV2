using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace Upload_GeralV1
{
    public class LeitorExcel
    {
        // Monta Excel CSV (Leitor csv)
        public DataTable MontaExcelCsv(string caminhoArquivo)
        {

            DataTable ExcelDataTable = new DataTable();
            // DataSet
            DataSet DataSet = new DataSet();
            // Cria DataColumn base
            DataColumn ColunaLeitura;
            List<DataRow> ListaLinhas = new List<DataRow>();
            DataRow itemLinha;

            string[] linhasCsv = File.ReadAllLines(caminhoArquivo, Encoding.GetEncoding(28591));

            string cabecalhoCSV = linhasCsv[0];

            // Cria Cabeçalho DataTable excel
                var dadosSeparados = cabecalhoCSV.Split(";");
                foreach(string dados in dadosSeparados)
                {
                    ColunaLeitura = new DataColumn();
                    ColunaLeitura.DataType = System.Type.GetType("System.String");
                    ColunaLeitura.ColumnName = dados;
                    ColunaLeitura.ReadOnly = true;
                    ExcelDataTable.Columns.Add(ColunaLeitura);
                }
            //Le todas as linhas da planilha 
            DataSet.Tables.Add(ExcelDataTable);

            for (int rowNumber = 1; rowNumber < linhasCsv.Length; rowNumber++)
            {
                string[] linha = linhasCsv[rowNumber].ToString().Split(";");
                itemLinha = ExcelDataTable.NewRow();
                int posicao = 0;
                 foreach(string cabecalho in dadosSeparados)
                 {
                    itemLinha[cabecalho] =  linha[posicao];
                    posicao++;

                 }
                ExcelDataTable.Rows.Add(itemLinha);
            }        

            return ExcelDataTable;
        }



        //Monta excel no DataTable 
        public DataTable MontaExcelDinamico(Stream arquivoExcel)
        {
            IWorkbook workBook = new HSSFWorkbook();
            workBook.MissingCellPolicy = MissingCellPolicy.CREATE_NULL_AS_BLANK;
            workBook = new HSSFWorkbook(arquivoExcel);
            ISheet linhas;
            // Cria datatable
            DataTable ExcelDataTable = new DataTable();
            // DataSet
            DataSet DataSet = new DataSet();
            // Cria DataColumn base
            DataColumn ColunaLeitura;
            List<DataRow> ListaLinhas = new List<DataRow>();
            DataRow itemLinha;
            // Pega todos os cabeçalhos do documento
            linhas = workBook.GetSheetAt(0);
            var cabecalhos = linhas.GetRow(0);
            //Pega valor de celula a celula 
            foreach (var linha in cabecalhos.Cells)
            {
                ColunaLeitura = new DataColumn();
                ColunaLeitura.DataType = System.Type.GetType("System.String");
                ColunaLeitura.ColumnName = linha.StringCellValue.ToString().Trim();
                ColunaLeitura.ReadOnly = true;
                ExcelDataTable.Columns.Add(ColunaLeitura);
            }
            //Confirma adicoes no DataTable
            DataSet.Tables.Add(ExcelDataTable);
            // Lê linha a linha a partir da primeira linha apos o cabeçalho
            for (int rowNumber = 1; rowNumber <= linhas.LastRowNum; rowNumber++)
            {
                var row = linhas.GetRow(rowNumber);
                itemLinha = ExcelDataTable.NewRow();
                var posicao = 0;
                foreach (var itensCabecalhos in cabecalhos.Cells)
                {
                    try
                    {
                        itemLinha[itensCabecalhos.ToString()] = row.GetCell(posicao, MissingCellPolicy.RETURN_NULL_AND_BLANK).ToString();
                    }
                    catch (Exception)
                    {
                        itemLinha[itensCabecalhos.ToString()] = "";
                    }
                    posicao++;
                }
                ExcelDataTable.Rows.Add(itemLinha);
            }
            workBook.Close();
            return ExcelDataTable;
        }
    }
}
