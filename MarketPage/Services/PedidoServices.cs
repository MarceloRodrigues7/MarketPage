using ADO;
using System.Collections.Generic;
using System.Linq;
using static MarketPage.Models.ResponseMercadoPagoGetOrder;

namespace MarketPage.Services
{
    public class PedidoServices
    {
        public readonly List<string> StatusIgnore = new() { "aprovado", "rejeitado", "cancelado", "devolvido", "cobrado de volta", "finalizado", "preparando", "enviado", "entregue" };

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

        public string GetStatusPagamentoPedido(Root pedidoMercadoPago)
        {
            var statusPagamento = pedidoMercadoPago.Elements.First();
            return statusPagamento.Payments.Last().Status;
        }
    }
}
