using ProjetoQuadra.Models;

namespace ProjetoQuadra.Data.Repositorio.Interfaces
{
    public interface ICardapioRepositorio
    {
        List<Cardapio> BuscarTodos();
        void AdicionarItem(Cardapio item);
        void RemoverItem(int id);

    }
}
