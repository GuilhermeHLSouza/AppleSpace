using Applespace.Models;

namespace Applespace.Repositorio.Carrinho
{
    public interface ICarrinhoRepositorio
    {
        public void AdicionarCarrinho(int codBarra, int quantidade, int idCliente);
        
        public List<Carrinhos> ListarCarrinho(int id);
        public void RemoverCarrinho(int id);
    }
}
