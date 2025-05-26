using Microsoft.AspNetCore.Mvc;
using Applespace.Models;
using Applespace.Repositorio.Produto;
using MySql.Data.MySqlClient;
using Applespace.Data;
using Newtonsoft.Json;

namespace Applespace.Controllers
{
    public class AdminController : Controller
    {
        private readonly IProdutoRepositorio _produtoRepositorio;
        private readonly Database _db;

        public AdminController(IProdutoRepositorio produtoRepositorio, Database db)
        {
            _produtoRepositorio = produtoRepositorio;
            _db = db;
        }

        // 🔥 Verifica se o usuário é admin
        private bool UsuarioEhAdmin()
        {
            var usuarioLogado = JsonConvert.DeserializeObject<Clientes>(
                HttpContext.Session.GetString("usuarioLogado") ?? ""
            );

            return usuarioLogado != null && usuarioLogado.Adm;
        }

        // ✅ DASHBOARD
        public IActionResult Dashboard()
        {
            if (!UsuarioEhAdmin()) return RedirectToAction("Index", "Home");

            using var conn = _db.GetConnection();

            var cmdProdutos = new MySqlCommand("SELECT COUNT(*) FROM Produtos", conn);
            int totalProdutos = Convert.ToInt32(cmdProdutos.ExecuteScalar());

            var cmdCupons = new MySqlCommand("SELECT COUNT(*) FROM Cupons WHERE Ativo = 1", conn);
            int totalCuponsAtivos = Convert.ToInt32(cmdCupons.ExecuteScalar());

            ViewBag.TotalProdutos = totalProdutos;
            ViewBag.TotalCuponsAtivos = totalCuponsAtivos;

            return View();
        }

        // ✅ PRODUTOS

        [HttpGet]
        public IActionResult CadastrarProdutos()
        {
            if (!UsuarioEhAdmin()) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public IActionResult CadastrarProdutos(Produtos produto)
        {
            if (!UsuarioEhAdmin()) return RedirectToAction("Index", "Home");
            _produtoRepositorio.Adicionar(produto);
            return RedirectToAction(nameof(Produtos));
        }

        public IActionResult Produtos()
        {
            if (!UsuarioEhAdmin()) return RedirectToAction("Index", "Home");
            var lista = _produtoRepositorio.MostrarProdutos();
            return View(lista);
        }

        [HttpGet]
        public IActionResult Alterar(int id)
        {
            if (!UsuarioEhAdmin()) return RedirectToAction("Index", "Home");
            var produto = _produtoRepositorio.ListarProduto(id);
            return View(produto);
        }

        [HttpPost]
        public IActionResult AlterarProduto(Produtos produto)
        {
            if (!UsuarioEhAdmin()) return RedirectToAction("Index", "Home");
            _produtoRepositorio.EditarProdutos(produto);
            return RedirectToAction(nameof(Produtos));
        }

        [HttpGet]
        public IActionResult Remover(int id)
        {
            if (!UsuarioEhAdmin()) return RedirectToAction("Index", "Home");
            var produto = _produtoRepositorio.ListarProduto(id);
            return View(produto);
        }

        public IActionResult Excluir(int id)
        {
            if (!UsuarioEhAdmin()) return RedirectToAction("Index", "Home");
            _produtoRepositorio.RemoverProdutos(id);
            return RedirectToAction(nameof(Produtos));
        }

        // ✅ CUPONS

        [HttpGet]
        public IActionResult CadastrarCupons()
        {
            if (!UsuarioEhAdmin()) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public IActionResult CadastrarCupons(Cupom model)
        {
            if (!UsuarioEhAdmin()) return RedirectToAction("Index", "Home");

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

        public IActionResult Cupons()
        {
            if (!UsuarioEhAdmin()) return RedirectToAction("Index", "Home");

            // 🔥 Atualiza status dos expirados
            using (var conn = _db.GetConnection())
            {
                string sql = "UPDATE Cupons SET Ativo = 0 WHERE Expiracao < NOW()";
                var cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }

            // 🔍 Carrega os cupons
            List<Cupom> lista = new List<Cupom>();

            using (var conn = _db.GetConnection())
            {
                var cmd = new MySqlCommand("SELECT * FROM Cupons ORDER BY Expiracao DESC", conn);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var cupom = new Cupom
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Codigo = reader["Codigo"].ToString(),
                        Tipo = reader["Tipo"].ToString(),
                        Valor = Convert.ToDecimal(reader["Valor"]),
                        Expiracao = Convert.ToDateTime(reader["Expiracao"]),
                        Ativo = Convert.ToBoolean(reader["Ativo"])
                    };

                    lista.Add(cupom);
                }
            }

            return View(lista);
        }

        // 🔁 Alterar Status (Ativo/Desativo)
        [HttpPost]
        public IActionResult AlterarStatusCupom(int id)
        {
            if (!UsuarioEhAdmin()) return RedirectToAction("Index", "Home");

            using (var conn = _db.GetConnection())
            {
                string selectSql = "SELECT Ativo FROM Cupons WHERE Id = @id";
                var selectCmd = new MySqlCommand(selectSql, conn);
                selectCmd.Parameters.AddWithValue("@id", id);

                var statusAtual = Convert.ToBoolean(selectCmd.ExecuteScalar());
                bool novoStatus = !statusAtual;

                string updateSql = "UPDATE Cupons SET Ativo = @ativo WHERE Id = @id";
                var updateCmd = new MySqlCommand(updateSql, conn);
                updateCmd.Parameters.AddWithValue("@ativo", novoStatus);
                updateCmd.Parameters.AddWithValue("@id", id);

                updateCmd.ExecuteNonQuery();
            }

            return RedirectToAction("Cupons");
        }
    }
}
