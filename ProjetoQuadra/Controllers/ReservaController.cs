using Microsoft.AspNetCore.Mvc;
using ProjetoQuadra.Data;
using ProjetoQuadra.Data.Repositorio;
using ProjetoQuadra.Data.Repositorio.Interfaces;
using ProjetoQuadra.Models;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Claims;

namespace ProjetoQuadra.Controllers
{
    public class ReservaController : Controller
    {
        private readonly ILogger<ReservaController> _logger;
        private readonly IReservasRepositorio _reservasRepositorio;
        private readonly IUsuariosRepositorio _usuariosRepositorio;
        private readonly IQuadrasRepositorio _quadrasRepositorio;

        private readonly List<TimeSpan> _horariosPossiveis = new List<TimeSpan>
    {
        TimeSpan.FromHours(13),
        TimeSpan.FromHours(14).Add(TimeSpan.FromMinutes(30)),
        TimeSpan.FromHours(16),
        TimeSpan.FromHours(17).Add(TimeSpan.FromMinutes(30)),
        TimeSpan.FromHours(19),
        TimeSpan.FromHours(20).Add(TimeSpan.FromMinutes(30)),
        TimeSpan.FromHours(22)
    };
        private readonly TimeSpan _duracaoUltimoSlot = TimeSpan.FromMinutes(90);

        public ReservaController(
            ILogger<ReservaController> logger,
            IReservasRepositorio reservasRepositorio,
            IUsuariosRepositorio usuariosRepositorio,
            IQuadrasRepositorio quadrasRepositorio)
        {
            _logger = logger;
            _reservasRepositorio = reservasRepositorio;
            _usuariosRepositorio = usuariosRepositorio;
            _quadrasRepositorio = quadrasRepositorio;
        }
        private TimeSpan ToTimeSpan(string horaString)
        {
            // Tenta fazer o parse da string "hh:mm" para TimeSpan
            if (TimeSpan.TryParseExact(horaString, @"hh\:mm", CultureInfo.InvariantCulture, out TimeSpan result))
            {
                return result;
            }
            // Tenta um parse mais genérico como fallback
            if (TimeSpan.TryParse(horaString, out result))
            {
                return result;
            }
            return TimeSpan.Zero;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int quadraId = 1)
        {
            int usuarioLogadoId = 0;
            if (User.Identity.IsAuthenticated)
            {
                int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out usuarioLogadoId);
            }

            DateTime dataConsulta = DateTime.Today;
            var todasQuadras = (await _quadrasRepositorio.GetAllQuadras()).ToList();
            var quadraSelecionada = todasQuadras.FirstOrDefault(q => q.id_quadra == quadraId)
                                    ?? todasQuadras.FirstOrDefault();

            if (quadraSelecionada == null)
                return Content("Nenhuma quadra cadastrada.");

            var reservasDoDiaParaQuadra = await _reservasRepositorio.GetReservasPorQuadraEData(quadraSelecionada.id_quadra, dataConsulta);

            var slotsDisponiveis = new List<SlotDisponibilidade>();
            for (int i = 0; i < _horariosPossiveis.Count; i++)
            {
                var horaInicioSlot = _horariosPossiveis[i];
                TimeSpan horaFimSlot = (i < _horariosPossiveis.Count - 1)
                    ? _horariosPossiveis[i + 1]
                    : horaInicioSlot.Add(_duracaoUltimoSlot);

                var slot = new SlotDisponibilidade
                {
                    HorarioInicio = horaInicioSlot.ToString(@"hh\:mm"),
                    HorarioFim = horaFimSlot.ToString(@"hh\:mm"),
                    QuadraId = quadraSelecionada.id_quadra,
                    DataSelecionada = dataConsulta.ToString("yyyy-MM-dd"),
                    PodeReservar = true,
                    Status = "Livre"
                };

                foreach (var reserva in reservasDoDiaParaQuadra)
                {
                    if (horaInicioSlot < reserva.hora_fim && reserva.hora_inicio < horaFimSlot)
                    {
                        slot.PodeReservar = false;
                        slot.Status = (usuarioLogadoId > 0 && reserva.id_usuario == usuarioLogadoId) ? "Seu" : "Reservado";
                        break;
                    }
                }

                slotsDisponiveis.Add(slot);
            }
            var reservasDoUsuario = (await _reservasRepositorio.GetReservasPorUsuario(usuarioLogadoId))
                                     .OrderByDescending(r => r.data_reserva)
                                     .ToList();

            var viewModel = new ReservasViewModel
            {
                Quadras = todasQuadras,
                QuadraSelecionada = quadraSelecionada,
                Usuarios = await _usuariosRepositorio.GetUsuarioPorId(usuarioLogadoId),

                ReservasDoDia = reservasDoDiaParaQuadra.OrderBy(r => r.hora_inicio).ToList(),
                Reservas = reservasDoUsuario,

                SlotsDisponiveis = slotsDisponiveis,
                DataConsulta = dataConsulta
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Alugar(int quadraId = 1, string dataSelecionada = null)
        {
            int usuarioLogadoId = 0;
            if (User.Identity.IsAuthenticated)
            {
                if (int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int parsedId))
                {
                    usuarioLogadoId = parsedId;
                }
            }
            
            DateTime dataConsulta = DateTime.Now.Date;
            if (!string.IsNullOrEmpty(dataSelecionada) && DateTime.TryParse(dataSelecionada, out DateTime parsedDate))
            {
                dataConsulta = parsedDate.Date;
            }

            var reservasExistentes = await _reservasRepositorio.GetReservasPorQuadraEData(quadraId, dataConsulta);
            var slotsDisponiveis = new List<SlotDisponibilidade>();

            for (int i = 0; i < _horariosPossiveis.Count; i++)
            {
                var horaInicioSlot = _horariosPossiveis[i];
                TimeSpan horaFimSlot;

                if (i < _horariosPossiveis.Count - 1)
                {
                    horaFimSlot = _horariosPossiveis[i + 1];
                }
                else
                {
                    horaFimSlot = horaInicioSlot.Add(_duracaoUltimoSlot);
                }

                var slot = new SlotDisponibilidade
                {
                    HorarioInicio = horaInicioSlot.ToString(@"hh\:mm"),
                    HorarioFim = horaFimSlot.ToString(@"hh\:mm"),
                    QuadraId = quadraId,
                    DataSelecionada = dataConsulta.ToString("yyyy-MM-dd"),
                    PodeReservar = true,
                    Status = "Livre"
                };
                foreach (var reserva in reservasExistentes)
                {
                    if (horaInicioSlot < reserva.hora_fim && reserva.hora_inicio < horaFimSlot)
                    {
                        slot.PodeReservar = false;

                        if (usuarioLogadoId > 0 && reserva.id_usuario == usuarioLogadoId)
                        {
                            slot.Status = "Seu";
                        }
                        else
                        {
                            slot.Status = "Reservado";
                        }
                        break;
                    }
                }
                slotsDisponiveis.Add(slot);
            }

            var viewModel = new ReservasViewModel
            {
                QuadraSelecionada = new Quadras { id_quadra = quadraId, nome = $"Quadra {quadraId}" },
                DataConsulta = dataConsulta,
                SlotsDisponiveis = slotsDisponiveis,
                FeedbackMessage = TempData["Success"]?.ToString() ?? TempData["Error"]?.ToString(),
                IsSuccessMessage = TempData["Success"] != null
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReservarSlot(SlotDisponibilidade slotDados)
        {
            TimeSpan.TryParse(slotDados.HorarioInicio, out TimeSpan horaInicio);
            TimeSpan.TryParse(slotDados.HorarioFim, out TimeSpan horaFim);
            DateTime.TryParse(slotDados.DataSelecionada, out DateTime dataReserva);

            var usuarioLogadoId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            int quadraId = slotDados.QuadraId;

            var reservasExistentes = await _reservasRepositorio.GetReservasPorQuadraEData(quadraId, dataReserva.Date);
            var sobreposicao = reservasExistentes.Any(reserva =>
                (horaInicio < reserva.hora_fim && reserva.hora_inicio < horaFim)
            );

            if (sobreposicao)
            {
                TempData["Error"] = "Este horário não está mais disponível.";
                return RedirectToAction(nameof(Alugar), new { quadraId = quadraId, dataSelecionada = slotDados.DataSelecionada });
            }

            var novaReserva = new Reservas
            {
                id_usuario = usuarioLogadoId, 
                id_quadra = quadraId,    
                data_reserva = dataReserva.Date,
                hora_inicio = horaInicio,
                hora_fim = horaFim,
                status = "confirmada"
            };

            await _reservasRepositorio.AddReserva(novaReserva);

            TempData["Success"] = "Reservado com Sucesso!";
            return RedirectToAction(nameof(Alugar), new { quadraId = quadraId, dataSelecionada = slotDados.DataSelecionada });
        }
    }
}
