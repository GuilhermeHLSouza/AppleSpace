using System.Diagnostics;
using Applespace.Data;
using Applespace.Libraries.LoginClientes;
using Applespace.Models;
using Applespace.Repositorio.Carrinho;
using Applespace.Repositorio.Login;
using Applespace.Repositorio.Produto;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Applespace.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Database _db;
        private IProdutoRepositorio? _produtoRepositorio;
        private ILoginRepositorio? _loginRepositorio;
        private LoginClientes _LoginClientes;
        private ICarrinhoRepositorio? _carrinhoRepositorio;

        public HomeController(
            ILogger<HomeController> logger,
            IProdutoRepositorio produto,
            ILoginRepositorio login,
            LoginClientes loginClientes,
            ICarrinhoRepositorio carrinho,
            Database db)
        {
            _logger = logger;
            _produtoRepositorio = produto;
            _loginRepositorio = login;
            _LoginClientes = loginClientes;
            _carrinhoRepositorio = carrinho;
            _db = db;
        }

        public IActionResult Index(string search)
        {
            List<Produtos> produto = new List<Produtos>();

            using (MySqlConnection conn = _db.GetConnection())
            {
                string sql = "SELECT * FROM Produtos";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        produto.Add(new Produtos
                        {
                            codBarra = reader.GetInt32("Cod_Barra"),
                            valor = reader.GetDecimal("Preco"),
                            descricao = reader.GetString("Descricao"),
                            nome = reader.GetString("Nome"),
                            img = reader.GetString("Img"),
                            estoque = reader.GetInt32("Estoque"),
                            Id_Cate = reader.GetInt32("Id_Cate")
                        });
                    }
                }
            }

            if (!string.IsNullOrEmpty(search))
            {
                produto = produto
                    .Where(p => p.nome != null && p.nome.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return View(produto);
        }

        public IActionResult Produto(int id)
        {
            Produtos produto = null;
            using (MySqlConnection conn = _db.GetConnection())
            {
                string sql = @"SELECT * FROM Produtos WHERE Produtos.Cod_Barra = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        produto = new Produtos
                        {
                            codBarra = reader.GetInt32("Cod_Barra"),
                            valor = reader.GetDecimal("Preco"),
                            descricao = reader.GetString("Descricao"),
                            nome = reader.GetString("Nome"),
                            img = reader.GetString("Img"),
                            estoque = reader.GetInt32("Estoque")
                        };
                    }
                }
            }

            return View(produto);
        }

        public IActionResult AdicionarCarrinho() => View();

        [HttpPost]
        public IActionResult AdicionarCarrinho(int codBarra, int? quantidade)
        {
            var cliente = _LoginClientes.GetCliente();
            if (cliente == null)
            {
                return RedirectToAction("Login");
            }

            int qtd = quantidade ?? 1;
            _carrinhoRepositorio?.AdicionarCarrinho(codBarra, qtd, cliente.idCliente);

            return RedirectToAction("Produto", new { id = codBarra });
        }

        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(Clientes cli)
        {
            Clientes loginDB = _loginRepositorio.Login(cli.email, cli.senha);

            if (loginDB != null)
            {
                _LoginClientes.Login(loginDB);
                return RedirectToAction("Perfil");
            }

            ViewData["msg"] = "Usuário inválido, verifique e-mail e senha";
            return View();
        }

        public IActionResult Cadastrar() => View();

        [HttpPost]
        public IActionResult Cadastrar(Clientes cliente)
        {
            _loginRepositorio.cadastrar(cliente);
            return View(nameof(Login));
        }

        public IActionResult Perfil()
        {
            Clientes cliente = _LoginClientes.GetCliente();

            if (cliente != null)
            {
                using (MySqlConnection conn = _db.GetConnection())
                {
                    string sql = @"SELECT * FROM Clientes WHERE Email = @email and Senha = @senha";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@email", cliente.email);
                    cmd.Parameters.AddWithValue("@senha", cliente.senha);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            cliente = new Clientes
                            {
                                idCliente = reader.GetInt32("Id_Cliente"),
                                senha = reader.GetString("Senha"),
                                nome = reader.GetString("Nome"),
                                email = reader.GetString("Email"),
                                CPF = reader.GetInt32("Cpf"),
                                telefone = reader.GetInt32("Telefone")
                            };
                            _LoginClientes.Logout();
                            _LoginClientes.Login(cliente);
                            return View(cliente);
                        }
                        else
                        {
                            return View(nameof(Login));
                        }
                    }
                }
            }

            return RedirectToAction("Login");
        }

        public IActionResult AlterarPerfil()
        {
            Clientes clientes = _LoginClientes.GetCliente();
            return View(clientes);
        }

        public IActionResult Alterar(Clientes clientes)
        {
            _loginRepositorio.AtualizarCliente(clientes);
            _LoginClientes.Logout();
            _LoginClientes.Login(clientes);
            return RedirectToAction("Perfil");
        }

        public IActionResult SairPerfil()
        {
            _LoginClientes.Logout();
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
