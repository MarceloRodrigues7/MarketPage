using ADO;
using MarketPage.Models;
using System.Collections.Generic;

namespace MarketPage.Repository
{
    public interface IItemRepository
    {
        List<Item> GetItens();
        Item GetItem(long id);
        long GetIdItem(string nome);
        long PostItem(Item item);
        void DeleteItem(long id);
        Item GeraItem(ViewItemAdmAddEEdit item);
        void AtualizaItem(ViewItemAdmAddEEdit produto);
    }
}
