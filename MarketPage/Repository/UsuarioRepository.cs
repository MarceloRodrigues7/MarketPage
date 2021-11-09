using MarketPage.Context;
using MarketPage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPage.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        public Usuario GetUsuario(int idUsuario)
        {
            using (var context = new ContextEF())
            {
                return context.Usuarios.Where(u => u.Id == idUsuario).FirstOrDefault();
            };
        }
        public Usuario GetUsuario(string username, string password)
        {
            using (var context = new ContextEF())
            {
                return context.Usuarios.Where(u => u.Username == username && u.Password == password).FirstOrDefault();
            };
        }

        public Usuario GetUsuario(string username, string email, string telefone)
        {
            using (var context = new ContextEF())
            {
                return context.Usuarios.Where(u => u.Username == username && u.Email == email && u.Telefone == telefone).FirstOrDefault();
            };
        }

        public void PostUsuario(Usuario usuario)
        {
            using (var context = new ContextEF())
            {
                context.Usuarios.Add(usuario);
                context.SaveChanges();
            };
        }
        public bool ValidaNovoUsuario(string username)
        {
            using (var context = new ContextEF())
            {
                return context.Usuarios.Where(l => l.Username == username).Any();
            };
        }

        public void PutUsuario(Usuario usuario)
        {
            using (var context = new ContextEF())
            {
                context.Usuarios.Update(usuario);
                context.SaveChanges();
            };
        }
    }
}
