using ADO;
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
        public List<CodPromocao> GetList()
        {
            using (var context = new ContextEF())
            {
                return context.CodPromocoes.ToList();
            };
        }

        public CodPromocao GetPromocaoValida(string codigo)
        {
            using (var context = new ContextEF())
            {
                return context.CodPromocoes.
                    Where(c => c.Codigo == codigo && c.DataInicio <= DateTime.Now && c.DataFinal >= DateTime.Now && c.Ativo == true).
                    FirstOrDefault();
            };
        }

        public CodPromocao GetCodPromocao(CodPromocao codPromocao)
        {
            using (var context = new ContextEF())
            {
                return context.CodPromocoes.Where(c => c.Codigo == codPromocao.Codigo).FirstOrDefault();
            };
        }

        public CodPromocao GetCodPromocao(long id)
        {
            return GetCodPromocaoId(id);
        }

        public CodPromocao GetPromocaoUtilizada(long idCarrinho)
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

        public void Post(CodPromocao cod)
        {
            using (var context = new ContextEF())
            {
                context.CodPromocoes.Add(cod);
                context.SaveChanges();
            };
        }

        public void Put(CodPromocao cod)
        {
            var codLast = GetCodPromocao(cod);
            cod = AlteraValores(cod, codLast);
            using (var context = new ContextEF())
            {
                context.CodPromocoes.Update(cod);
                context.SaveChanges();
            };
        }

        public void Delete(long id)
        {
            var data = GetCodPromocaoId(id);
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

        private CodPromocao GetCodPromocaoId(long id)
        {
            using (var context = new ContextEF())
            {
                return context.CodPromocoes.Where(c => c.Id == id).FirstOrDefault();
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
