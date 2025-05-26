using Applespace.Libraries.LoginClientes;
using Applespace.Models;
using Applespace.Repositorio.Carrinho;
using Microsoft.AspNetCore.Mvc;

namespace Applespace.Controllers
{
    public class CarrinhoController : Controller
    {
        private readonly ICarrinhoRepositorio _carrinhoRepositorio;
        private readonly LoginClientes _loginClientes;

        public CarrinhoController(ICarrinhoRepositorio carrinhoRepositorio, LoginClientes loginClientes)
        {
            _carrinhoRepositorio = carrinhoRepositorio;
            _loginClientes = loginClientes;
        }

        public IActionResult Index()
        {
            var usuario = _loginClientes.GetUsuario();

            if (usuario != null)
            {
                var lista = _carrinhoRepositorio.ListarCarrinho(usuario.IdCliente);
                return View(lista);
            }

            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public IActionResult AdicionarCarrinho(int codBarra, int? quantidade)
        {
            var usuario = _loginClientes.GetUsuario();

            if (usuario == null)
            {
                return RedirectToAction("Login", "Login");
            }

            int qtd = quantidade ?? 1;

            _carrinhoRepositorio.AdicionarCarrinho(codBarra, qtd, usuario.IdCliente);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RemoverProdutos(int id)
        {
            _carrinhoRepositorio.RemoverQtdCarrinho(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AdicionarProdutos(int id)
        {
            _carrinhoRepositorio.AdicionarQtdCarrinho(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeletarCarrinho(int id)
        {
            _carrinhoRepositorio.RemoverCarrinho(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult ValidarCupom(string codigo)
        {
            var cupom = _carrinhoRepositorio.BuscarCupomPorCodigo(codigo);

            if (cupom == null)
            {
                return Json(new { valido = false });
            }

            return Json(new
            {
                valido = true,
                tipo = cupom.Tipo,
                valor = cupom.Valor
            });
        }
        // 🔹 Adiciona e fica na página
        [HttpPost]
        public IActionResult AdicionarSemRedirecionar(int codBarra, int? quantidade)
        {
            var usuario = _loginClientes.GetUsuario();
            if (usuario == null)
            {
                return RedirectToAction("Login", "Login");
            }

            int qtd = quantidade ?? 1;
            _carrinhoRepositorio.AdicionarCarrinho(codBarra, qtd, usuario.IdCliente);

            // Volta para a mesma página do produto
            return RedirectToAction("Produto", "Home", new { id = codBarra });
        }

        [HttpPost]
        public IActionResult AdicionarERedirecionar(int codBarra, int? quantidade)
        {
            var usuario = _loginClientes.GetUsuario();
            if (usuario == null)
            {
                return RedirectToAction("Login", "Login");
            }

            int qtd = quantidade ?? 1;
            _carrinhoRepositorio.AdicionarCarrinho(codBarra, qtd, usuario.IdCliente);

            // Redireciona direto para o carrinho
            return RedirectToAction("Index", "Carrinho");
        }
    }
}
