using MarketPage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPage.Repository
{
    public interface IUsuarioRepository
    {
        bool ValidaNovoUsuario(string username);
        Usuario GetUsuario(int idUsuario);
        Usuario GetUsuario(string username, string password);
        Usuario GetUsuario(string username, string email, string telefone);
        void PostUsuario(Usuario usuario);
        void PutUsuario(Usuario usuario);
    }
}
