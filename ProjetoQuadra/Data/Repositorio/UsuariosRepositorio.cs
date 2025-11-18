using Microsoft.EntityFrameworkCore;
using ProjetoQuadra.Data;
using ProjetoQuadra.Data.Repositorio.Interfaces;
using ProjetoQuadra.Models;

namespace ProjetoQuadra.Data.Repositorio
{
    public class UsuariosRepositorio : IUsuariosRepositorio
    {
        private readonly BancoContexto _bancoContext;

        public UsuariosRepositorio(BancoContexto bancoContext)
        {
            _bancoContext = bancoContext;
        }
        public void CadastrarUsuario(Usuarios usuarios)
        {
            _bancoContext.Usuarios.Add(usuarios);
            _bancoContext.SaveChanges();
        }
        public Usuarios ValidarUsuario(Usuarios usuarios)
        {
            return _bancoContext.Usuarios.FirstOrDefault(u => u.cpf == usuarios.cpf && u.senha == usuarios.senha);
        }
        public async Task<Usuarios> GetUsuarioPorId(int usuarioId)
        {
            return await _bancoContext.Usuarios.FirstOrDefaultAsync(u => u.id_usuario == usuarioId);
        }
    }
}
