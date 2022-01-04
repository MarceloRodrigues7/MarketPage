﻿using MarketPage.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPage.Context
{
    public class ContextEF : DbContext
    {
        private readonly static string StringConnectionDbTeste = "Data Source=DESKTOP-R0PQL34;Initial Catalog=MarketPage;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private readonly static string StringConnectionDbProd = "Data Source=mssql-58318-0.cloudclusters.net,12564;Initial Catalog=MarketPage;User ID=adm;Password=P@ssword11;Connect Timeout=1000;TrustServerCertificate=True";
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
    }
}
