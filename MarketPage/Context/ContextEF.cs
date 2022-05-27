using ADO;
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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<FormaPagamento>().HasData(new FormaPagamento
            {
                Id = 1,
                Nome = "MercadoPago",
                TokenClient = "APP_USR-8824013a-f82d-4879-a34a-634eaac91242",
                TokenService = "APP_USR-1223540178250481-092615-8aee4b2461ec8e00fd5f066bcbd83d26-194500220"
            });

            #region Usuario admin padrão
            modelBuilder.Entity<Usuario>().HasData(new Usuario
            {
                RoleAcess = "Admin",
                StatusAtivo = true,
                ConcordaRegras = true,
                DataNascimento = new DateTime(1990, 10, 01),
                Email = "admin@marketpage.com",
                Id = 1,
                Nome = "Admin",
                Password = "mudar123",
                PermiteEmail = true,
                Sobrenome = "teste",
                Telefone = "10203040",
                Username = "admin"
            });
            #endregion

            #region Categoria padrão
            modelBuilder.Entity<Categoria>().HasData(new Categoria
            {
                Ativo = true,
                DataAdicao = DateTime.Now,
                Id = 1,
                Nome = "Destaques"
            });
            #endregion

            #region Valores Frete Teste
            modelBuilder.Entity<FreteValores>().HasData(new FreteValores
            {
                CepFinal = long.MaxValue,
                CepInicio = 0,
                Id = 1,
                PesoMax = 999,
                PesoMin = 0,
                PrazoMax = 999,
                PrazoMin = 0,
                PrecoFrete = 10,
                Servico = "PAC"
            });
            modelBuilder.Entity<FreteValores>().HasData(new FreteValores
            {
                CepFinal = long.MaxValue,
                CepInicio = 0,
                Id = 2,
                PesoMax = 999,
                PesoMin = 0,
                PrazoMax = 999,
                PrazoMin = 0,
                PrecoFrete = 15,
                Servico = "SEDEX"
            });
            #endregion
        }

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
        public DbSet<FormaPagamento> PlataformasPagamento { get; set; }
    }
}
