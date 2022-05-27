using ADO;
using MarketPage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPage.Repository
{
    public interface ICodPromocionalRepository
    {
        List<CodPromocao> GetList();
        CodPromocao GetPromocaoValida(string codigo);
        CodPromocao GetCodPromocao(CodPromocao codPromocao);
        CodPromocao GetCodPromocao(long id);
        void Post(CodPromocao cod);
        void Put(CodPromocao cod);
        void Delete(long id);
        void PostCodPromoUsuario(CodPromocaoUtilizado cod);
        CodPromocao GetPromocaoUtilizada(long idCarrinho);
    }
}
