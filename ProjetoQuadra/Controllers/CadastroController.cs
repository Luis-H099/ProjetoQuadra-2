using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ProjetoQuadra.Data.Repositorio.Interfaces;
using ProjetoQuadra.Models;
using System.Security.Claims;
using ProjetoQuadra.Utils;

namespace ProjetoQuadra.Controllers
{
    public class CadastroController : Controller
    {
        private readonly IUsuariosRepositorio _usuariosRepositorio;

        public CadastroController(IUsuariosRepositorio usuariosRepositorio)
        {
            _usuariosRepositorio = usuariosRepositorio;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {             
            return View(); 
        }
        public IActionResult Cadastrar()
        {
            return View();
        }
        public IActionResult ValidarUsuario(Usuarios usuarios)
        {
            string cpfLimpo = CpfUtils.LimparCpf(usuarios.cpf);

            if (string.IsNullOrEmpty(cpfLimpo) || cpfLimpo.Length != 11)
            {
                TempData["MsgErro"] = "O CPF deve ter 11 dígitos numéricos.";
                return View("Index");
            }
            usuarios.cpf = cpfLimpo;
            try
            {
                var retorno = _usuariosRepositorio.ValidarUsuario(usuarios);

                if (retorno != null)
                {
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, retorno.id_usuario.ToString()),
                new Claim(ClaimTypes.Name, retorno.nome),
            };

                    var claimsIdentity = new ClaimsIdentity(
                        claims,
                        CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
                    };
                    HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties).Wait();

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["MsgErro"] = "Cpf ou senha incorretos!!! Tente novamente...";
                }
            }
            catch (Exception)
            {
                TempData["MsgErro"] = "Erro ao buscar dados do usuário";
            }

            return View("Login", usuarios);
        }
        [HttpPost]
        public IActionResult CadastrarUsuario(Usuarios usuarios)
        {
            if (!CpfUtils.NomeNaoContemNumero(usuarios.nome))
            {
                TempData["MsgErro"] = "O Nome não pode conter números. Por favor, corrija.";
                return RedirectToAction("Cadastrar", "Cadastro");
            }

            string cpfLimpo = CpfUtils.LimparCpf(usuarios.cpf);

            if (cpfLimpo.Length != 11)
            {
                TempData["MsgErro"] = "O CPF deve conter exatamente 11 dígitos numéricos.";
                return RedirectToAction("Cadastrar"); 
            }
            usuarios.cpf = cpfLimpo;
            usuarios.nome = usuarios.nome.Trim();
            try
            {
                _usuariosRepositorio.CadastrarUsuario(usuarios);

                TempData["MsgSucesso"] = "Usuário cadastrado com sucesso!";
                return RedirectToAction("Index", "Cadastro");
            }
            catch (Exception ex)
            {
                TempData["MsgErro"] = "Erro ao cadastrar usuário. Tente novamente...";
            }
            return RedirectToAction("Cadastrar");
        }
    }
}

