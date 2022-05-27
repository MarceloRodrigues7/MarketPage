using ADO;
using MarketPage.Context;
using MarketPage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPage.Repository
{
    public class ItemRepository : IItemRepository
    {
        public List<Item> GetItens()
        {
            using (var context = new ContextEF())
            {
                return context.Itens.ToList();
            };
        }
        public Item GetItem(long id)
        {
            using (var context = new ContextEF())
            {
                return context.Itens.Where(i => i.Id == id).First();
            };
        }
        public long GetIdItem(string nome)
        {
            using (var context = new ContextEF())
            {
                var res = context.Itens.Where(i => i.Nome == nome).FirstOrDefault();
                if (res != null)
                {
                    return res.Id;
                }
                return 0;
            };
        }
        public long PostItem(Item item)
        {
            using (var context = new ContextEF())
            {
                context.Itens.Add(item);
                context.SaveChanges();
                return GetIdItem(item.Nome);
            };
        }
        public void DeleteItem(long id)
        {
            using (var context = new ContextEF())
            {
                context.Itens.RemoveRange(context.Itens.Where(i => i.Id == id));
                context.SaveChanges();
            };
        }
        public Item GeraItem(ViewItemAdmAddEEdit item)
        {
            return new Item
            {
                Nome = item.Nome,
                Descricao = item.Descricao,
                Valor = decimal.Parse(item.ValorString.Replace(".", ",")),
                Tamanhos = item.Tamanhos,
                Quantidade = item.Quantidade,
                Destaque = item.Destaque,
                DataAdicao = DateTime.Now,
                IdCategoria = item.IdCategoria,
                Peso = float.Parse(item.PesoString.Replace(".", ","))
            };
        }
        public void AtualizaItem(ViewItemAdmAddEEdit produto)
        {
            using (var context = new ContextEF())
            {
                var prod = context.Itens.Where(i => i.Id == produto.Id).FirstOrDefault();
                prod.Nome = produto.Nome;
                prod.Descricao = produto.Descricao;
                prod.Valor = decimal.Parse(produto.ValorString.Replace(".", ","));
                prod.Tamanhos = produto.Tamanhos;
                prod.Quantidade = produto.Quantidade;
                prod.Destaque = produto.Destaque;
                prod.IdCategoria = produto.IdCategoria;
                prod.Peso = float.Parse(produto.PesoString.Replace(".", ","));
                context.Itens.Update(prod);
                context.SaveChanges();
            };
        }
    }
}
