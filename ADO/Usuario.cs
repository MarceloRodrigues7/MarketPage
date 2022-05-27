using System;

namespace ADO
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string RoleAcess { get; set; }
        public bool StatusAtivo { get; set; }
        public bool PermiteEmail { get; set; }
        public bool ConcordaRegras { get; set; }
        public string Telefone { get; set; }

    }
}
