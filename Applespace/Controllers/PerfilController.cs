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

        public IActionResult Perfil()
        {
            var usuario = _loginClientes.GetUsuario();

            if (usuario == null)
            {
                return RedirectToAction("Login", "Login");
            }

            return View(usuario);
        }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AlterarPerfil(Clientes cliente)
        {
            var usuario = _loginClientes.GetUsuario();

            if (usuario == null)
            {
                return RedirectToAction("Login", "Login");
            }

            cliente.IdCliente = usuario.IdCliente;
            cliente.Adm = usuario.Adm;

            _loginRepositorio.AtualizarCliente(cliente);
            _loginClientes.Logout();
            _loginClientes.Login(cliente);

            return RedirectToAction("Perfil");
        }
    }
}
