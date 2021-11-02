using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPage.Models
{
    public class FretePedidoUsuario
    {
        public long Id { get; set; }
        public int IdUsuario { get; set; }
        public long IdCarrinho { get; set; }
        public string TipoFrete { get; set; }
        public decimal ValorTotal { get; set; }
    }
}
