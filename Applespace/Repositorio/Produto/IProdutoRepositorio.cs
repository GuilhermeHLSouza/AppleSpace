using Applespace.Models;

namespace Applespace.Repositorio.Produto
{
    public interface IProdutoRepositorio
    {
        public IEnumerable<Produtos> MostrarProdutos();
        public void EditarProdutos(Produtos produto);
        public void Adicionar(Produtos produto);
        public bool RemoverProdutos(int  id);
        public Produtos ListarProduto(int id);
    }
}
