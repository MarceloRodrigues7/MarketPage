using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO
{
    public class CodPromocao
    {
        public long Id { get; set; }
        public string Codigo { get; set; }
        public decimal Desconto { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFinal { get; set; }
        public bool Ativo { get; set; }
        public int Utilizacoes { get; set; }
    }
}
