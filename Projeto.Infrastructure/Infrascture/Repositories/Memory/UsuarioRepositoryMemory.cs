using Projeto.Domain;
namespace Projeto.Infrastructure.Infrascture.Repositories.Memory
{
    public class UsuarioRepositoryMemory
    {
        private readonly List<Usuario> _usuarios = new()
        {
            new Usuario{Nome = "andre",Senha = "1234" , Role = "User"},
            new Usuario{Nome = "Lucas", Senha = "3214", Role = "Admin"}
        };
        public Usuario BuscarPorNome(string nome)
        {
            return _usuarios.FirstOrDefault(u => u.Nome == nome);
        }

    }
}
