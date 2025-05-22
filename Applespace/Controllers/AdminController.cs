using Microsoft.AspNetCore.Mvc;
using Applespace.Models;
using Applespace.Repositorio.Produto;
using Applespace.Repositorio.Login;
using Newtonsoft.Json;
using MySql.Data.MySqlClient;
using Applespace.Data;

namespace Applespace.Controllers
{
    public class AdminController : Controller
    {
        private readonly IProdutoRepositorio? _produtoRepositorio;
        private readonly ILoginRepositorio? _loginRepositorio;
        private readonly IConfiguration _configuration;
        private readonly Database _db;

        public AdminController(IProdutoRepositorio produto, ILoginRepositorio loginRepositorio, IConfiguration configuration, Database db)
        {
            _produtoRepositorio = produto;
            _loginRepositorio = loginRepositorio;
            _configuration = configuration;
            _db = db;
        }


        public IActionResult Index()
        {
            return View();
        }


        public IActionResult PaginaAdm()
        {
            using var conn = _db.GetConnection();

            // Contagem de produtos
            var cmdProdutos = new MySqlCommand("SELECT COUNT(*) FROM Produtos", conn);
            int totalProdutos = Convert.ToInt32(cmdProdutos.ExecuteScalar());

            // Contagem de cupons ativos
            var cmdCupons = new MySqlCommand("SELECT COUNT(*) FROM Cupons WHERE Ativo = 1", conn);
            int totalCuponsAtivos = Convert.ToInt32(cmdCupons.ExecuteScalar());

            // Enviar os valores para a View
            ViewBag.TotalProdutos = totalProdutos;
            ViewBag.TotalCuponsAtivos = totalCuponsAtivos;

            return View();
        }

        [HttpGet]
        public IActionResult CadastrarProdutos()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CadastrarProdutos(Produtos produto)
        {
            _produtoRepositorio?.Adicionar(produto);
            return RedirectToAction(nameof(Produtos));
        }

        [HttpGet]
        public IActionResult Produtos()
        {
            var lista = _produtoRepositorio?.MostrarProdutos();
            return View(lista);
        }

        [HttpGet]
        public IActionResult Alterar(int id)
        {
            var produto = _produtoRepositorio?.ListarProduto(id);
            return View(produto);
        }

        [HttpPost]
        public IActionResult AlterarProduto(Produtos produto)
        {
            _produtoRepositorio?.EditarProdutos(produto);
            return RedirectToAction(nameof(Produtos));
        }

        [HttpGet]
        public IActionResult Remover(int id)
        {
            var produto = _produtoRepositorio?.ListarProduto(id);
            return View(produto);
        }

        public IActionResult Excluir(int id)
        {
            _produtoRepositorio?.RemoverProdutos(id);
            return RedirectToAction(nameof(Produtos));
        }

        [HttpGet]
        public IActionResult CadastrarCupons()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CadastrarCupons(Cupom model)
        {

            // ✅ Validação de tipo
            var tiposValidos = new[] { "porcentagem", "frete", "valorfixo" };
            if (!tiposValidos.Contains(model.Tipo?.ToLower()))
            {
                ViewBag.Mensagem = "Tipo inválido. Use: porcentagem, frete ou valorfixo.";
                return View();
            }

            using var conn = _db.GetConnection();
            var cmd = new MySqlCommand(@"
                INSERT INTO Cupons (Codigo, Tipo, Valor, Expiracao, Ativo)
                VALUES (@Codigo, @Tipo, @Valor, @Expiracao, @Ativo)", conn);

            cmd.Parameters.AddWithValue("@Codigo", model.Codigo);
            cmd.Parameters.AddWithValue("@Tipo", model.Tipo.ToLower());
            cmd.Parameters.AddWithValue("@Valor", model.Valor);
            cmd.Parameters.AddWithValue("@Expiracao", model.Expiracao);
            cmd.Parameters.AddWithValue("@Ativo", model.Ativo);

            cmd.ExecuteNonQuery();

            ViewBag.Mensagem = "Cupom cadastrado com sucesso!";
            return View();
        }

        [HttpGet]
        public IActionResult Cupons()
        {
            List<Cupom> lista = new List<Cupom>();

            using var conn = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            conn.Open();

            var cmd = new MySqlCommand("SELECT * FROM Cupons ORDER BY Expiracao DESC", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var cupom = new Cupom
                {
                    Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0,
                    Codigo = reader["Codigo"]?.ToString(),
                    Tipo = reader["Tipo"]?.ToString(),
                    Valor = reader["Valor"] != DBNull.Value ? Convert.ToDecimal(reader["Valor"]) : 0m,
                    Expiracao = reader["Expiracao"] != DBNull.Value ? Convert.ToDateTime(reader["Expiracao"]) : DateTime.MinValue,
                    Ativo = reader["Ativo"] != DBNull.Value && Convert.ToBoolean(reader["Ativo"])
                };

                lista.Add(cupom);
            }

            return View(lista);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("adminLogado");
            return View("~/Views/Home/Index.cshtml");
        }

    }
}
