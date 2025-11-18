using Microsoft.EntityFrameworkCore;
using ProjetoQuadra.Data.Repositorio.Interfaces;
using ProjetoQuadra.Models;

namespace ProjetoQuadra.Data.Repositorio
{
    public class QuadrasRepositorio : IQuadrasRepositorio
    {
        private readonly BancoContexto _bancoContext;

        public QuadrasRepositorio(BancoContexto bancoContext)
        {
            _bancoContext = bancoContext;
        }
        public async Task<List<Quadras>> GetAllQuadras()
        {
            return await _bancoContext.Quadras.ToListAsync();
        }
        public List<Quadras> BuscarTodasQuadras()
        {
            return _bancoContext.Quadras.ToList();
        }

    }
}
