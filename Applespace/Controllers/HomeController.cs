using System.Diagnostics;
using System.Runtime.Intrinsics.Arm;
using Applespace.Data;
using Applespace.Models;
using Applespace.Repositorio.Login;
using Applespace.Repositorio.Produto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using MySql.Data.MySqlClient;

namespace Applespace.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Database db = new Database();
        private IProdutoRepositorio? _produtoRepositorio;
        private ILoginRepositorio? _loginRepositorio;
        public HomeController(ILogger<HomeController> logger, IProdutoRepositorio produto, ILoginRepositorio login)
        {
            _logger = logger;
            _produtoRepositorio = produto;
            _loginRepositorio = login;
        }


        public IActionResult Index(string search)
        {
            List<Produtos> produto = new List<Produtos>();

            using (MySqlConnection conn = db.GetConnection())
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


        public IActionResult Produto(int id) {

            Produtos produto = null;
            List<Produtos> prod = new List<Produtos>();
            using (MySqlConnection conn = db.GetConnection())
            {
                string sql = @"
                                SELECT * FROM Produtos
                                WHERE Produtos.Cod_Barra = @id";
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

        public IActionResult AdicionarCarrinho (){



            return View();         
        }

        public IActionResult Login()
        {
            return View();
        }
        public static int x;
        [HttpPost]
        public IActionResult Login(Clientes cli)
        {
            Clientes loginDB = _loginRepositorio.Login(cli.email, cli.senha);

            if (loginDB != null)
            {
                x = loginDB.idCliente;
                return RedirectToAction("Perfil");
            }
            else
            {
                ViewData["msg"] = "Usuário inválido, verifique e-mail e senha";
                return View();
            }
        }
        public IActionResult Cadastrar() 
        {
            return View();
        }
        [HttpPost]
        public IActionResult Cadastrar(Clientes cliente)
        {
            _loginRepositorio.cadastrar(cliente);
            return View(nameof(Login));
        }

        public IActionResult Perfil()
        {
            Clientes cliente = null;
            using (MySqlConnection conn = db.GetConnection())
            {
                string sql = @"SELECT * FROM Clientes WHERE Clientes.Id_Cliente = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", x);
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
                        return View(cliente);
                    }
                    else
                    {
                        return View(nameof(Login));
                    }
                }
            }          

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
