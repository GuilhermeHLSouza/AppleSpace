using Applespace.Models;

namespace Applespace.Repositorio.Compra
{
    public interface ICompraRepositorio
    {
        public void RegistroEndereço(int idCliente, int cep, int numero, string rua, string bairro, string complemento);
        public void SelectUsuario(Clientes cliente);
        public void FormaPgm();
    }
}
