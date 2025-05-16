using Applespace.Libraries.LoginClientes;
using Applespace.Repositorio.Carrinho;
using Microsoft.AspNetCore.Mvc;

namespace Applespace.Controllers
{
    public class CarrinhoController : Controller
    {
        private ICarrinhoRepositorio? _carrinhoRepositorio;
        private LoginClientes _LoginClientes;
        public CarrinhoController(ICarrinhoRepositorio carrinhoRepositorio, LoginClientes loginClientes)
        {
            _carrinhoRepositorio = carrinhoRepositorio;
            _LoginClientes = loginClientes;
        }
        public IActionResult Index()
        {
            var cliente = _LoginClientes.GetCliente();
            if (cliente != null)
            {
                var lista = _carrinhoRepositorio?.ListarCarrinho(cliente.idCliente);
                return View(lista);
            }
            else
            {
                return Redirect("Home/Login");
            }

        }

        public IActionResult DeletarCarrinho()
        {
            return View();
        }
        [HttpPost]
        public IActionResult DeletarCarrinho(int id)
        {
            _carrinhoRepositorio?.RemoverCarrinho(id);
            return RedirectToAction("Index");
        }

        public IActionResult ComprarProdutos()
        {

            return View();
        }

        public IActionResult AdicionarProdutos()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AdicionarProdutos(int id)
        {
            _carrinhoRepositorio?.AdicionarQtdCarrinho(id);

            return RedirectToAction("Index");
        }
        public IActionResult RemoverProdutos(int id)
        {
            _carrinhoRepositorio?.RemoverQtdCarrinho(id);

            return RedirectToAction("Index");
        }
    }
}
