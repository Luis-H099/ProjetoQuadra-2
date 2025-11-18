using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoQuadra.Models
{
    public class Reservas
    {
        public int id_reserva { get; set; }
        public DateTime data_reserva { get; set; }
        public TimeSpan hora_inicio { get; set; }
        public TimeSpan hora_fim { get; set; }
        public string status { get; set; }
        public decimal valor_total { get; set; }
        public DateTime? data_criacao { get; set; }

        //Fk
        public int id_usuario { get; set; }
        public int id_quadra { get; set; }
        //Navegação
        [ForeignKey("id_usuario")]
        public Usuarios Usuario { get; set; }
        [ForeignKey("id_quadra")]
        public Quadras Quadra { get; set; }
    }
}
