using Microsoft.AspNetCore.Mvc;
using Applespace.Models;
using Applespace.Data;
using Applespace.Repositorio.Produto;
using Applespace.Repositorio.Login;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
//using Applespace.Repositorio.Categoria;

namespace Applespace.Controllers
{
    public class AdminController : Controller
    {
        private IProdutoRepositorio? _produtoRepositorio;
       // private ICategoriaRepositorio? _categoriaRepositorio;
       private ILoginRepositorio? _loginRepositorio;

        public AdminController(IProdutoRepositorio produto /*ICategoriaRepositorio cate*/, ILoginRepositorio loginRepositorio) {
            _produtoRepositorio = produto;
          //  _categoriaRepositorio = cate;
          _loginRepositorio = loginRepositorio;
        }
        
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(Administradores Adm)
        {
            Administradores loginDB = _loginRepositorio.LoginAdm(Adm.Nome, Adm.Senha, Adm.IdAdm);

            if (loginDB != null)
            {                
                return RedirectToAction(nameof(PaginaAdm));
            }
            else
            {
                ViewData["msg"] = "Usuário inválido, verifique e-mail e senha";
                return View();
            }
        }
        public IActionResult PaginaAdm()
        {
            return View(_produtoRepositorio?.MostrarProdutos());
        }

        public IActionResult CadastrarProdutos()
        {
            
            return View();
        }

        [HttpPost]
        public IActionResult CadastrarProdutos(Produtos produto, Categorias categoria)
        {
            _produtoRepositorio?.Adicionar(produto);
            return RedirectToAction(nameof(PaginaAdm));
        }

        public IActionResult Remover(int id)
        {
            Produtos produto = _produtoRepositorio?.ListarProduto(id);
            return View(produto);
        }
        public IActionResult Excluir(int id)
        {
            _produtoRepositorio.RemoverProdutos(id);
            return RedirectToAction(nameof(PaginaAdm));
        }
        public IActionResult Alterar(int id)
        {
            Produtos produto = _produtoRepositorio?.ListarProduto(id);
            return View(produto);
        }

        [HttpPost]
        public IActionResult AlterarProduto(Produtos produto)
        {
            _produtoRepositorio.EditarProdutos(produto);
            return RedirectToAction(nameof(PaginaAdm));
        }

    }
}
