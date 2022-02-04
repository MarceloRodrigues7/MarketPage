using MarketPage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPage.Repository
{
    public interface ICodPromocionalRepository
    {
        List<CodPromocao> GetCodPromocoes();
        CodPromocao GetCodPromocao(string codigo);
        CodPromocao GetCodPromocao(CodPromocao codPromocao);
        void PostCodPromocao(CodPromocao cod);
        void PutCodPromocao(CodPromocao cod);
        void DeleteCodPromocao(string cod);
        void PostCodPromoUsuario(CodPromocaoUtilizado cod);
        CodPromocao GetCodPromocao(long idCarrinho);
    }
}
