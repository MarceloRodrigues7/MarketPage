using MarketPage.Context;
using MarketPage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPage.Repository
{
    public class CodPromocionalRepository : ICodPromocionalRepository
    {
        public List<CodPromocao> GetCodPromocoes()
        {
            using (var context = new ContextEF())
            {
                return context.CodPromocoes.ToList();
            };
        }

        public CodPromocao GetCodPromocao(string codigo)
        {
            using (var context = new ContextEF())
            {
                return context.CodPromocoes.Where(c => c.Codigo == codigo && c.DataInicio <= DateTime.Now && c.DataFinal >= DateTime.Now && c.Ativo == true).FirstOrDefault();
            };
        }
        public CodPromocao GetCodPromocao(long idCarrinho)
        {
            using (var context = new ContextEF())
            {
                var data = context.CodPromoUsuarios.Where(c => c.IdCarrinho == idCarrinho).FirstOrDefault();
                if (data == null)
                {
                    return null;
                }
                return GetCodPromocaoId(data.IdCodPromocao);
            };
        }

        private CodPromocao GetCodPromocaoId(long id)
        {
            using (var context = new ContextEF())
            {
                return context.CodPromocoes.Where(c => c.Id == id).FirstOrDefault();
            };
        }

        public void PostCodPromocao(CodPromocao cod)
        {
            using (var context = new ContextEF())
            {
                context.CodPromocoes.Add(cod);
                context.SaveChanges();
            };
        }

        public void PutCodPromocao(CodPromocao cod)
        {
            var codLast = GetCodPromocao(cod.Codigo);
            cod = AlteraValores(cod, codLast);
            using (var context = new ContextEF())
            {
                context.CodPromocoes.Update(cod);
                context.SaveChanges();
            };
        }

        public void DeleteCodPromocao(string cod)
        {
            var data = GetCodPromocao(cod);
            using (var context = new ContextEF())
            {
                context.CodPromocoes.Remove(data);
                context.SaveChanges();
            };
        }

        public void PostCodPromoUsuario(CodPromocaoUtilizado cod)
        {
            using (var context = new ContextEF())
            {
                context.CodPromoUsuarios.Add(cod);
                context.SaveChanges();
            };
        }

        private static CodPromocao AlteraValores(CodPromocao cod, CodPromocao codLast)
        {
            return new CodPromocao
            {
                Id = codLast.Id,
                Codigo = cod.Codigo,
                Ativo = cod.Ativo,
                DataFinal = cod.DataFinal,
                DataInicio = cod.DataInicio,
                Desconto = cod.Desconto,
                Utilizacoes = cod.Utilizacoes
            };
        }
    }
}
