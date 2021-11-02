using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPage.Models
{
    public class Carrinho
    {
        public long Id { get; set; }
        public int IdUsuario { get; set; }
        public long IdItem { get; set; }
        public decimal Valor { get; set; }
        public string Tamanhos { get; set; }
        public int Quantidade { get; set; }
        public DateTime DataHora { get; set; }
        public long? IdPedido { get; set; }
    }
}
