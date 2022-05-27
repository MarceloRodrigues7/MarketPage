using ADO;
using MarketPage.Models;

namespace MarketPage.Repository
{
    public interface IFormaPagamentoRepository
    {
        FormaPagamento GetFormaPagamento();
        void Update(FormaPagamento formaPagamento);
    }
}
