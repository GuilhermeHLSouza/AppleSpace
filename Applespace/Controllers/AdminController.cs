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

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Administradores Adm)
        {
            var loginDB = _loginRepositorio.LoginAdm(Adm.Nome, Adm.Senha, Adm.IdAdm);
            if (loginDB != null)
            {
                HttpContext.Session.SetString("adminLogado", JsonConvert.SerializeObject(loginDB));
                return RedirectToAction(nameof(PaginaAdm));
            }

            ViewData["msg"] = "Usuário inválido, verifique nome, senha ou ID.";
            return View();
        }

        public IActionResult PaginaAdm()
        {
            if (!AdminAutenticado()) return RedirectToAction(nameof(Index));
            return View();
        }

        [HttpGet]
        public IActionResult CadastrarProdutos()
        {
            if (!AdminAutenticado()) return RedirectToAction(nameof(Index));
            return View();
        }

        [HttpPost]
        public IActionResult CadastrarProdutos(Produtos produto)
        {
            if (!AdminAutenticado()) return RedirectToAction(nameof(Index));
            _produtoRepositorio?.Adicionar(produto);
            return RedirectToAction(nameof(Produtos));
        }

        [HttpGet]
        public IActionResult Produtos()
        {
            if (!AdminAutenticado()) return RedirectToAction(nameof(Index));
            var lista = _produtoRepositorio?.MostrarProdutos();
            return View(lista);
        }

        [HttpGet]
        public IActionResult Alterar(int id)
        {
            if (!AdminAutenticado()) return RedirectToAction(nameof(Index));
            var produto = _produtoRepositorio?.ListarProduto(id);
            return View(produto);
        }

        [HttpPost]
        public IActionResult AlterarProduto(Produtos produto)
        {
            if (!AdminAutenticado()) return RedirectToAction(nameof(Index));
            _produtoRepositorio?.EditarProdutos(produto);
            return RedirectToAction(nameof(Produtos));
        }

        [HttpGet]
        public IActionResult Remover(int id)
        {
            if (!AdminAutenticado()) return RedirectToAction(nameof(Index));
            var produto = _produtoRepositorio?.ListarProduto(id);
            return View(produto);
        }

        public IActionResult Excluir(int id)
        {
            if (!AdminAutenticado()) return RedirectToAction(nameof(Index));
            _produtoRepositorio?.RemoverProdutos(id);
            return RedirectToAction(nameof(Produtos));
        }

        [HttpGet]
        public IActionResult CadastrarCupons()
        {
            if (!AdminAutenticado()) return RedirectToAction(nameof(Index));
            return View();
        }

        [HttpPost]
        public IActionResult CadastrarCupons(Cupom model)
        {
            if (!AdminAutenticado()) return RedirectToAction(nameof(Index));

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
            if (!AdminAutenticado()) return RedirectToAction(nameof(Index));

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
            return RedirectToAction(nameof(Index));
        }

        private bool AdminAutenticado()
        {
            return !string.IsNullOrEmpty(HttpContext.Session.GetString("adminLogado"));
        }
    }
}
