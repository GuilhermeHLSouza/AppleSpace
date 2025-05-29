using Microsoft.AspNetCore.Mvc;
using Applespace.Models;
using Applespace.Repositorio.Login;
using Applespace.Libraries.LoginClientes;

namespace Applespace.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginRepositorio _loginRepositorio;
        private readonly LoginClientes _loginClientes;

        public LoginController(ILoginRepositorio loginRepositorio, LoginClientes loginClientes)
        {
            _loginRepositorio = loginRepositorio;
            _loginClientes = loginClientes;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(Clientes cliente)
        {
            var usuarioDB = _loginRepositorio.Login(cliente.Email, cliente.Senha, cliente.CPF);

            if (usuarioDB != null)
            {
                _loginClientes.Login(usuarioDB);

                if (usuarioDB.Adm)
                {
                    return RedirectToAction("Dashboard", "Admin");
                }
                else
                {
                    return RedirectToAction("Perfil", "Perfil");
                }
            }

            TempData["msg"] = "E-mail ou senha inválidos.";
            return View();
        }

        [HttpGet]
        public IActionResult Cadastrar() => View();

        [HttpPost]
        public IActionResult Cadastrar(Clientes cliente)
        {
            var existe = _loginRepositorio.BuscarPorEmail(cliente.Email);

            if (existe != null)
            {
                TempData["msg"] = "E-mail já cadastrado!";
                return View();
            }

            _loginRepositorio.Cadastrar(cliente);
            TempData["msg"] = "Cadastro realizado com sucesso!";
            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            _loginClientes.Logout();
            return RedirectToAction("Index", "Home");
        }
    }
}
