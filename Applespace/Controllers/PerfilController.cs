using Microsoft.AspNetCore.Mvc;
using Applespace.Libraries.LoginClientes;
using Applespace.Models;
using Applespace.Repositorio.Login;

namespace Applespace.Controllers
{
    public class PerfilController : Controller
    {
        private readonly LoginClientes _loginClientes;
        private readonly ILoginRepositorio _loginRepositorio;

        public PerfilController(LoginClientes loginClientes, ILoginRepositorio loginRepositorio)
        {
            _loginClientes = loginClientes;
            _loginRepositorio = loginRepositorio;
        }

        // ✅ Página de Visualização do Perfil
        public IActionResult Perfil()
        {
            var usuario = _loginClientes.GetUsuario();

            if (usuario == null)
            {
                return RedirectToAction("Login", "Login");
            }

            return View(usuario);
        }

        // ✅ Página de Alteração do Perfil (GET)
        [HttpGet]
        [HttpGet]
        public IActionResult AlterarPerfil()
        {
            var usuario = _loginClientes.GetUsuario();

            if (usuario == null)
            {
                return RedirectToAction("Login", "Login");
            }

            return View(usuario);
        }


        // ✅ Post da Alteração dos Dados do Perfil
        [HttpPost]
        public IActionResult Alterar(Clientes cliente)
        {
            var usuario = _loginClientes.GetUsuario();

            if (usuario == null)
            {
                return RedirectToAction("Login", "Login");
            }

            cliente.IdCliente = usuario.IdCliente;
            _loginRepositorio.AtualizarCliente(cliente);

            _loginClientes.Logout();
            _loginClientes.Login(cliente);

            return RedirectToAction("Perfil");
        }
    }
}
