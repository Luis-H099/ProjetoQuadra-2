using System.Globalization;

namespace ProjetoQuadra.Models
{
    public class Usuarios
    {
        public int id_usuario { get; set; }
        public string nome { get; set; }
        public string cpf { get; set; }
        public string senha { get; set; }
        public DateTime? data_cadastro { get; set; }
    }
}