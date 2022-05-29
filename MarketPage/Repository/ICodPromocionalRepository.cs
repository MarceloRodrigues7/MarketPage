using ADO;
using System.Collections.Generic;

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
