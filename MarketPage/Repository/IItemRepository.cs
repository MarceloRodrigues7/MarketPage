using MarketPage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
