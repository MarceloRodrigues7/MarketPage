using ADO;
using System.Collections.Generic;

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
