using ProjetoQuadra.Data.Repositorio.Interfaces;
using ProjetoQuadra.Models;

namespace ProjetoQuadra.Data.Repositorio
{
    public class CardapioRepositorio : ICardapioRepositorio
    {
        private readonly BancoContexto _bancoContext;

        public CardapioRepositorio(BancoContexto bancoContext)
        {
            _bancoContext = bancoContext;
        }
        
        public List<Cardapio> BuscarTodos()
        {
            return _bancoContext.Cardapio.ToList();
        }

        public void AdicionarItem(Cardapio item)
        {
            _bancoContext.Cardapio.Add(item);
            _bancoContext.SaveChanges();
        }

        public void RemoverItem(int id)
        {
            var item = _bancoContext.Cardapio.Find(id);
            if (item != null)
            {
                _bancoContext.Cardapio.Remove(item);
                _bancoContext.SaveChanges();
            }
        }
    }
}
