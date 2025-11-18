namespace ProjetoQuadra.Models
{
    public class Cardapio
    {
        public int id_item { get; set; }
        public string nome_item { get; set; }
        public decimal preco { get; set; }
        public string categoria { get; set; }
        public bool disponibilidade { get; set; }
    }
}
