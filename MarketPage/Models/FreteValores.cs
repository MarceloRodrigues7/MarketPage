using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPage.Models
{
    public class FreteValores
    {
        public long Id { get; set; }
        public long CepInicio { get; set; }
        public long CepFinal { get; set; }
        public int PesoMin { get; set; }
        public int PesoMax { get; set; }
        public decimal PrecoFrete { get; set; }
        public int PrazoMin { get; set; }
        public int PrazoMax { get; set; }
        public string Servico { get; set; }
    }
}
