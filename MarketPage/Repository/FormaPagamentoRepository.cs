using MarketPage.Context;
using MarketPage.Models;
using System;
using System.Linq;

namespace MarketPage.Repository
{
    public class FormaPagamentoRepository : IFormaPagamentoRepository
    {
        public FormaPagamento GetFormaPagamento()
        {
            using (var context = new ContextEF())
            {
                try
                {
                    return context.PlataformasAtendimento.First();
                }
                catch (Exception)
                {
                    context.Add(new FormaPagamento());
                    context.SaveChanges();
                    return GetFormaPagamento();
                }                
            };
        }

        public void Update(FormaPagamento formaPagamento)
        {
            using (var context = new ContextEF())
            {
                var data = context.PlataformasAtendimento.First();
                context.PlataformasAtendimento.Remove(data);
                context.PlataformasAtendimento.Add(formaPagamento);
                context.SaveChanges();
            };
        }
    }
}
