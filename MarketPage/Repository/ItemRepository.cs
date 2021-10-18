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
                return context.Itens.Where(i => i.Nome == nome).First().Id;
            };
        }
        public void PostItem(Item item)
        {
            using (var context = new ContextEF())
            {
                context.Itens.Add(item);
                context.SaveChanges();
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
        public Item GeraItem(ItemImagem item)
        {
            return new Item
            {
                Nome = item.Nome,
                Descricao = item.Descricao,
                Valor = decimal.Parse(item.Valor.Replace(".", ",")),
                Tamanhos = item.Tamanhos,
                Quantidade = item.Quantidade,
                Destaque = item.Destaque,
                DataAdicao = DateTime.Now,
                IdCategoria = item.Categoria
            };
        }
        public void AtualizaItem(ItemImagem produto)
        {
            using (var context = new ContextEF())
            {
                var prod = context.Itens.Where(i => i.Id == produto.Id).FirstOrDefault();
                prod.Nome = produto.Nome;
                prod.Descricao = produto.Descricao;
                prod.Valor = decimal.Parse(produto.Valor.Replace(".", ","));
                prod.Tamanhos = produto.Tamanhos;
                prod.Quantidade = produto.Quantidade;
                prod.Destaque = produto.Destaque;
                prod.IdCategoria = produto.Categoria;
                context.Itens.Update(prod);
                context.SaveChanges();
            };
        }
    }
}
