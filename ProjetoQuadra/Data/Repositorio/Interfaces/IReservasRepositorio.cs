using ProjetoQuadra.Models;

namespace ProjetoQuadra.Data.Repositorio.Interfaces
{
    public interface IReservasRepositorio
    {
        Task<IEnumerable<Reservas>> GetReservasPorQuadraEData(int quadraId, DateTime data);
        Task AddReserva(Reservas reserva);
        Task<IEnumerable<Reservas>> GetReservasPorUsuario(int usuarioId);
    }
}
