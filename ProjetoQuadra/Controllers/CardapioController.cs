using Microsoft.AspNetCore.Mvc;
using ProjetoQuadra.Data.Repositorio;
using ProjetoQuadra.Data.Repositorio.Interfaces;
using ProjetoQuadra.Models;
using WebApplication1.Controllers;

namespace ProjetoQuadra1.Controllers
{
    public class CardapioController : Controller
    {
        private readonly ILogger<CardapioController> _logger;
        private readonly IQuadrasRepositorio _quadrasRepositorio;
        private readonly ICardapioRepositorio _cardapioRepositorio;

        public CardapioController(
            ILogger<CardapioController> logger,
            IQuadrasRepositorio quadrasRepositorio,
            ICardapioRepositorio cardapioRepositorio)
        {
            _logger = logger;
            _quadrasRepositorio = quadrasRepositorio;
            _cardapioRepositorio = cardapioRepositorio;
        }

        public IActionResult Index()
        {
            var vm = new CardapioViewModel
            {
                Cardapio = _cardapioRepositorio.BuscarTodos(),
                Quadras = _quadrasRepositorio.BuscarTodasQuadras()
            };

            return View(vm);
        }
        public IActionResult Admin()
        {
            var vm = new CardapioViewModel
            {
                Cardapio = _cardapioRepositorio.BuscarTodos()
            };

            return View(vm);
        }
        public IActionResult AdicionarItem(Cardapio item)
        {
            try
            {
                _cardapioRepositorio.AdicionarItem(item);
                TempData["MensagemSucesso"] = "Item do cardápio adicionado com sucesso!";
                return RedirectToAction("Admin");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao adicionar item do cardápio: {ex.Message}";
                return RedirectToAction("Admin");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoverItem(int id)
        {
            try
            {
                _cardapioRepositorio.RemoverItem(id);
                TempData["MensagemSucesso"] = "Item do cardápio removido com sucesso!";
                return RedirectToAction("Admin");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao remover item do cardápio: {ex.Message}";
                return RedirectToAction("Admin");
            }
        }
    }
}
