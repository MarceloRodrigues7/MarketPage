using MarketPage.Context;
using MarketPage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPage.Repository
{
    public class MessagesContatoRepository : IMessagesContatoRepository
    {
        public void PostMensage(MessageContato message)
        {
            using (var context = new ContextEF())
            {
                message.DataEnvio = DateTime.Now;
                message.Visualizado = false;
                context.MessagesContato.Add(message);
                context.SaveChanges();
            };
        }

        public List<MessageContato> GetMessageContatos()
        {
            using (var context = new ContextEF())
            {
                return context.MessagesContato.ToList();
            };
        }

        public void PostConfirmaVisualizacao(MessageContato message)
        {
            using (var context = new ContextEF())
            {
                message.DataVisualizado = DateTime.Now;
                message.Visualizado = true;
                context.MessagesContato.Update(message);
                context.SaveChanges();
            };
        }
    }
}
