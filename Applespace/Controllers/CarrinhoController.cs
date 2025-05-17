using Applespace.Libraries.LoginClientes;
using Applespace.Repositorio.Carrinho;
using Applespace.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Applespace.Controllers
{
    public class CarrinhoController : Controller
    {
        private readonly ICarrinhoRepositorio? _carrinhoRepositorio;
        private readonly LoginClientes _LoginClientes;

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

        // ✅ Método para validar cupom usando CarrinhoRepositorio
        [HttpGet]
        public JsonResult ValidarCupom(string codigo)
        {
            var cupom = _carrinhoRepositorio?.BuscarCupomPorCodigo(codigo);

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
    }
}
