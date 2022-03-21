using MarketPage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPage.Repository
{
    public interface IPedidoRepository
    {
        Pedido GetPedido(long idPedido);
        List<Pedido> GetPedidos();
        List<Pedido> GetPedidos(string status);
        List<Pedido> GetPedidos(int idUsuario);
        List<PedidoStatus> GetPedidosStatus();
        long PostPedido(Pedido pedido);
        void PutPedido(Pedido pedido);
        void PutStatusPedido(Pedido pedido);
    }
}
