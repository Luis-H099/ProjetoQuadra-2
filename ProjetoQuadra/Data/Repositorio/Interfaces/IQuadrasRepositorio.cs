using ProjetoQuadra.Models;

namespace ProjetoQuadra.Data.Repositorio.Interfaces
{
    public interface IQuadrasRepositorio
    {
        Task<List<Quadras>> GetAllQuadras();
        List<Quadras> BuscarTodasQuadras();
    }
}
