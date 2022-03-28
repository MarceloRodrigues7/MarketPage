using MarketPage.Models;
using System.Collections.Generic;
using System.Linq;

namespace MarketPage.Services
{
    public class PedidoServices
    {
        public List<int> ResumoTotalPedidos(List<Pedido> pedidos)
        {
            var resumo = new List<int>
            {
                pedidos.Count,
                pedidos.Where(p => p.StatusAtual == "pendente").Count(),
                pedidos.Where(p => p.StatusAtual == "aprovado").Count(),
                pedidos.Where(p => p.StatusAtual == "autorizado").Count(),
                pedidos.Where(p => p.StatusAtual == "em processo").Count(),
                pedidos.Where(p => p.StatusAtual == "em mediação").Count(),
                pedidos.Where(p => p.StatusAtual == "rejeitado").Count(),
                pedidos.Where(p => p.StatusAtual == "cancelado").Count(),
                pedidos.Where(p => p.StatusAtual == "devolvido").Count(),
                pedidos.Where(p => p.StatusAtual == "cobrado de volta").Count(),
                pedidos.Where(p => p.StatusAtual == "finalizado").Count(),
                pedidos.Where(p => p.StatusAtual == "preparando").Count(),
                pedidos.Where(p => p.StatusAtual == "enviado").Count(),
                pedidos.Where(p => p.StatusAtual == "entregue").Count()
            };
            return resumo;
        }
    }
}
