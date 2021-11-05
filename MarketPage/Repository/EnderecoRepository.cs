using MarketPage.Context;
using MarketPage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPage.Repository
{
    public class EnderecoRepository : IEnderecoRepository
    {
        public Endereco GetEndereco(int idUsuario)
        {
            using (var context = new ContextEF())
            {
                return context.EnderecosUsuario.Where(e => e.IdUsuario == idUsuario).FirstOrDefault();
            };
        }
    }
}
