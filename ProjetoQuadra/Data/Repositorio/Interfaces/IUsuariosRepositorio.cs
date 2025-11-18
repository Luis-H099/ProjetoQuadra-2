using ProjetoQuadra.Models;

namespace ProjetoQuadra.Data.Repositorio.Interfaces
{
    public interface IUsuariosRepositorio
    {
        void CadastrarUsuario(Usuarios usuarios);
        Task<Usuarios> GetUsuarioPorId(int usuarioId);
        Usuarios ValidarUsuario(Usuarios usuarios);
    }
}
