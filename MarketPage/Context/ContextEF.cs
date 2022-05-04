using MarketPage.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPage.Context
{
    public class ContextEF : DbContext
    {
        private readonly static string StringConnectionDbTeste = @"Data Source=ACER;Initial Catalog=MarketPage;User ID='sa';Password='p@ssword';Connect Timeout=999899999";
        //private readonly static string StringConnectionDbProd = @"Data Source=143.244.190.244;Initial Catalog=MarketPage;User ID='sa';Password='P@ssword';Connect Timeout=999899999";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(StringConnectionDbTeste);

        //Tabelas
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Item> Itens { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<ImgItem> ImagensItem { get; set; }
        public DbSet<MessageContato> MessagesContato { get; set; }
        public DbSet<Carrinho> CarrinhoItem { get; set; }
        public DbSet<Endereco> EnderecosUsuario { get; set; }
        public DbSet<Pedido> PedidosUsuario { get; set; }
        public DbSet<CodPromocao> CodPromocoes { get; set; }
        public DbSet<CodPromocaoUtilizado> CodPromoUsuarios { get; set; }
        public DbSet<FretePedidoUsuario> FretesPedidosUsuarios { get; set; }
        public DbSet<FreteValores> FreteValores { get; set; }
        public DbSet<PedidoStatus> PedidosStatus { get; set; }
        public DbSet<FormaPagamento> PlataformasAtendimento { get; set; }
    }
}
