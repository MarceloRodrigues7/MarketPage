using MarketPage.Context;
using MarketPage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPage.Repository
{
    public class CarrinhoRepository : ICarrinhoRepository
    {
        public List<Carrinho> GetCarrinhos(long idPedido)
        {
            using (var context = new ContextEF())
            {
                return context.CarrinhoItem.Where(c => c.IdPedido == idPedido).ToList();
            };
        }
        public List<ItemViewProduto> GetItensCarrinhoView(int idUsuario)
        {
            var data = new List<Carrinho>();
            var lista = new List<ItemViewProduto>();

            using (var context = new ContextEF())
            {
                data = context.CarrinhoItem.Where(c => c.IdUsuario == idUsuario && c.IdPedido == null).ToList();
                foreach (var item in data)
                {
                    var codPromo = context.CodPromoUsuarios.Where(c => c.IdCarrinho == item.Id).FirstOrDefault();
                    var frete = context.FretesPedidosUsuarios.Where(f => f.IdCarrinho == item.Id).FirstOrDefault();
                    var itemView = new ItemViewProduto
                    {
                        Id = item.Id,
                        Nome = context.Itens.Where(i => i.Id == item.IdItem).FirstOrDefault().Nome,
                        Descricao = context.Itens.Where(i => i.Id == item.IdItem).FirstOrDefault().Descricao,
                        Valor = item.Valor,
                        Tamanhos = item.Tamanhos,
                        Quantidade = item.Quantidade,
                        Img = context.ImagensItem.Where(i => i.IdItem == item.IdItem).FirstOrDefault().Img,

                    };
                    if (codPromo != null)
                    {
                        itemView.CodPromocional = context.CodPromocoes.Where(c => c.Id == codPromo.IdCodPromocao).FirstOrDefault().Codigo;
                        itemView.ValorDesconto = context.CodPromocoes.Where(c => c.Id == codPromo.IdCodPromocao).FirstOrDefault().Desconto;
                    }
                    if (frete != null)
                    {
                        itemView.TipoFrete = frete.TipoFrete;
                        itemView.ValorFrete = frete.ValorTotal;
                    }
                    lista.Add(itemView);
                }
            };
            return lista;
        }

        public void DeleteItemCarrinhoAdmin(long idItem)
        {
            using (var context = new ContextEF())
            {
                var data = context.CarrinhoItem.Where(c => c.IdItem==idItem);
                context.RemoveRange(data);
                context.SaveChanges();
            };
        }
    }
}
