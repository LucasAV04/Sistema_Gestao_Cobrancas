using Projeto.Domain;
using Projeto.Infrastructure.Infrascture.Repositories.Memory;

namespace Projeto.Application
{
    public class AuthService
    {
        private readonly UsuarioRepositoryMemory _usuariosRepo;

        public AuthService(UsuarioRepositoryMemory usuariosRepo)
        {
            _usuariosRepo = usuariosRepo;
        }
        public Usuario Login(string nome, string senha)
        {
            var usuario = _usuariosRepo.BuscarPorNome(nome);
            if (usuario == null) return null;
            return usuario.Senha == senha ? usuario : null;
        }

    }
}
