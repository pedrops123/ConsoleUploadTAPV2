using System;
using System.Collections.Generic;
using System.Text;

namespace Upload_GeralV1.Models
{
    public class RetornoCargaFuncionarios
    {
        public KeyValuePair<List<Empregados>, int> QtdEmpregadosInseridos { get; set; }
        public KeyValuePair<List<Empregados>, int> QtdEmpregadosAtualizados { get; set; }
        public KeyValuePair<List<Cargo>, int> QtdCargoUpload { get; set; }
        public KeyValuePair<List<CentroDeCusto>, int> QtdCentroCustoUpload { get; set; }
        public KeyValuePair<List<Setor>, int> QtdSetorUpload { get; set; }
        public List<KeyValuePair<Empregados, string>> ErrosEmpregados { get; set; }
    }
}
