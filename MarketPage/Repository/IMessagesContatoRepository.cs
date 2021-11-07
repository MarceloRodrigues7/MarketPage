using MarketPage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPage.Repository
{
    public interface IMessagesContatoRepository
    {
        void PostMensage(MessageContato message);
        List<MessageContato> GetMessageContatos();
        void PostConfirmaVisualizacao(MessageContato message);
    }
}
