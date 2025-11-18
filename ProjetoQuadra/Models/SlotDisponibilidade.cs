namespace ProjetoQuadra.Models
{
    public class SlotDisponibilidade
    {
        public string HorarioInicio { get; set; } 
        public string HorarioFim { get; set; }  
        public string Status { get; set; }    
        public bool PodeReservar { get; set; }  
        public int QuadraId { get; set; }
        public string DataSelecionada { get; set; } 
    }
}
