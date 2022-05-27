using ADO;
using MarketPage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPage.Repository
{
    public interface IEnderecoRepository
    {
        Endereco GetEndereco(int idUsuario);
        void InsertOrUpdate(Endereco endereco);
    }
}
