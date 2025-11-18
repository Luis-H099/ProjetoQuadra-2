namespace ProjetoQuadra.Models
{
    public class ReservasViewModel
    {
        public List <Quadras> Quadras { get; set; }
        public List<Reservas> Reservas { get; set; }
        public Usuarios Usuarios { get; set; }
        
        public List<Reservas> ReservasDoDia { get; set; }
        public Quadras QuadraSelecionada { get; set; }
        public List<SlotDisponibilidade> SlotsDisponiveis { get; set; }
        public DateTime DataConsulta { get; set; }
        public string FeedbackMessage { get; set; }
        public bool IsSuccessMessage { get; set; }
    }

}