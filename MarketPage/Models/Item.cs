using ADO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarketPage.Models
{
    public class ItemView
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public decimal Valor { get; set; }
        public string Tamanhos { get; set; }
        public int Quantidade { get; set; }
        public byte[] Img { get; set; }
        public int IdCategoria { get; set; }
    }

    public class ItemViewProduto
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public int Quantidade { get; set; }
        public string Tamanhos { get; set; }
        public int IdCategoria { get; set; }
        public byte[] Img { get; set; }
        public List<byte[]> ImgsPadrao { get; set; }
        public string TipoFrete { get; set; }
        public decimal? ValorFrete { get; set; }
        public string CodPromocional { get; set; }
        public decimal? ValorDesconto { get; set; }

        public Item GeraObj()
        {
            return new Item
            {
                Id = this.Id,
                Quantidade = this.Quantidade,
                Tamanhos = this.Tamanhos,
                Valor = this.Valor
            };
        }
    }

    public class ItemViewAdmin
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public int Quantidade { get; set; }
        public bool Destaque { get; set; }
        public DateTime DataAdicao { get; set; }
        public string Categoria { get; set; }
        public decimal Peso { get; set; }

        public static ItemViewAdmin GeraObj(Item item, List<Categoria> categorias)
        {
            return new ItemViewAdmin
            {
                Id = item.Id,
                Nome = item.Nome,
                Descricao = item.Descricao,
                Valor = item.Valor,
                Quantidade = item.Quantidade,
                Destaque = item.Destaque,
                DataAdicao = item.DataAdicao,
                Categoria = categorias.Where(c => c.Id == item.IdCategoria).First().Nome
            };
        }
    }

    public class ItemMercadoPago
    {
        public long Id { get; set; }
        public string IdMp { get; set; }
    }

    public class ItemViewDescAdmin : Item
    {
        public string Tamanho { get; set; }

        public static ItemViewDescAdmin GeraObj(Item item, Carrinho carrinho)
        {
            return new ItemViewDescAdmin
            {
                Id = item.Id,
                DataAdicao = item.DataAdicao,
                Descricao = item.Descricao,
                Destaque = item.Destaque,
                IdCategoria = item.IdCategoria,
                Nome = item.Nome,
                Peso = item.Peso,
                Quantidade = carrinho.Quantidade,
                Tamanho = carrinho.Tamanhos,
                Tamanhos = item.Tamanhos,
                Valor = item.Valor
            };
        }
    }

    public class Pesquisa
    {
        public string Nome { get; set; }
    }
}
