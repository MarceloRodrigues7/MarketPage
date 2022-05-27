using ADO;
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
        MessageContato GetMessageContatos(long id);
        void PutConfirmaVisualizacao(MessageContato message);
    }
}
