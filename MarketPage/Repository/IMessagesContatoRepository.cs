using ADO;
using System.Collections.Generic;

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
