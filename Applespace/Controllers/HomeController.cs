using System.Diagnostics;
using Applespace.Data;
using Applespace.Models;
using Applespace.Repositorio.Produto;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Applespace.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Database _db;
        private readonly IProdutoRepositorio _produtoRepositorio;

        public HomeController(
            ILogger<HomeController> logger,
            IProdutoRepositorio produto,
            Database db)
        {
            _logger = logger;
            _produtoRepositorio = produto;
            _db = db;
        }

        // ✅ Página inicial com busca
        public IActionResult Index(string search)
        {
            List<Produtos> produtos = _produtoRepositorio.MostrarProdutos().ToList();

            if (!string.IsNullOrEmpty(search))
            {
                produtos = produtos
                    .Where(p => p.Nome != null && p.Nome.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return View(produtos);
        }

        // ✅ Página de detalhes de produto
        public IActionResult Produto(int id)
        {
            var produto = _produtoRepositorio.ListarProduto(id);

            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        // ✅ Página de erro
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
