using MarketPage.Context;
using MarketPage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPage.Repository
{
    public class PedidoRepository : IPedidoRepository
    {
        public Pedido GetPedido(long idPedido)
        {
            using (var context = new ContextEF())
            {
                return context.PedidosUsuario.Where(p => p.Id == idPedido).FirstOrDefault();
            };
        }

        public List<Pedido> GetPedidos()
        {
            using (var context = new ContextEF())
            {
                return context.PedidosUsuario.ToList();
            };
        }
        
        public List<Pedido> GetPedidos(string status)
        {
            using (var context = new ContextEF())
            {
                return context.PedidosUsuario.Where(p=>p.StatusAtual==status).ToList();
            };
        }

        public List<Pedido> GetPedidos(int idUsuario)
        {
            using (var context = new ContextEF())
            {
                return context.PedidosUsuario.Where(p => p.IdUsuario == idUsuario).ToList();
            };
        }

        public long PostPedido(Pedido pedido)
        {
            using (var context = new ContextEF())
            {
                context.PedidosUsuario.Add(pedido);
                context.SaveChanges();
                var idPedido = context.PedidosUsuario.Where(p => p.DataRealizacao == pedido.DataRealizacao && p.ValorTotal == pedido.ValorTotal).FirstOrDefault().Id;
                return idPedido;
            };
        }

        public void PutStatusPedido(Pedido pedido)
        {
            using (var context = new ContextEF())
            {
                pedido.StatusAtual = context.PedidosStatus.Where(p => p.NomeMercadoLivre == pedido.StatusAtual).FirstOrDefault().Nome;
                if (pedido.StatusAtual == "Aprovado")
                {
                    pedido.DateFinalizacao = DateTime.UtcNow.AddHours(-3);
                }
                context.PedidosUsuario.Update(pedido);
                context.SaveChanges();
            };
        }
    }
}
