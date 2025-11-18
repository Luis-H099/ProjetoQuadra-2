using Microsoft.EntityFrameworkCore;
using ProjetoQuadra.Data.Repositorio.Interfaces;
using ProjetoQuadra.Models;

namespace ProjetoQuadra.Data.Repositorio
{
    public class ReservasRepositorio : IReservasRepositorio
    {
        private readonly BancoContexto _bancoContext;
        public ReservasRepositorio(BancoContexto bancoContext)
        {
            _bancoContext = bancoContext;
        }

        public async Task<IEnumerable<Reservas>> GetReservasPorQuadraEData(int quadraId, DateTime data)
        {
            var dataSemTempo = data.Date;

            return await _bancoContext.Reservas
                .Where(r => r.id_quadra == quadraId && r.data_reserva.Date == dataSemTempo)
                .ToListAsync();
        }
        public async Task AddReserva(Reservas reserva)
        {
            _bancoContext.Reservas.Add(reserva);
            await _bancoContext.SaveChangesAsync();
        }
        public async Task<IEnumerable<Reservas>> GetReservasPorUsuario(int usuarioId)
        {
            return await _bancoContext.Reservas
                .Include(q => q.Quadra)
                .Where(q => q.id_usuario == usuarioId)
                .ToListAsync();
        }
    }
}
