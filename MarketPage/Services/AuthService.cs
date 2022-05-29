using ADO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace MarketPage.Services
{
    public class AuthService
    {
        public void GeraIdentity(Usuario usuario, HttpContext httpContext)
        {
            var claims = GeraListClaim(usuario.Id.ToString(), usuario.RoleAcess);
            var propriedadesDeAutenticacao = GeraAuthProperties();

            var identidadeDeUsuario = new ClaimsIdentity(claims, "Login");
            ClaimsPrincipal claimPrincipal = new(identidadeDeUsuario);

            httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal, propriedadesDeAutenticacao);
        }

        private static AuthenticationProperties GeraAuthProperties() => new()
        {
            AllowRefresh = true,
            ExpiresUtc = DateTime.Now.ToLocalTime().AddHours(2),
            IsPersistent = true
        };

        private static List<Claim> GeraListClaim(string idUsuario, string roleAcessUsuario)
        {
            var claims = new List<Claim>();

            Claim claimName = new(ClaimTypes.Name, idUsuario);
            Claim claimRole = new(ClaimTypes.Role, roleAcessUsuario);

            claims.Add(claimName);
            claims.Add(claimRole);

            return claims;
        }

    }
}
