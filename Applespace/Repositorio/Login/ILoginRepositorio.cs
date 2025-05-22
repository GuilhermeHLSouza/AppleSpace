using Applespace.Models;

namespace Applespace.Repositorio.Login
{
    public interface ILoginRepositorio
    {
        public Clientes Login(string email, string senha);
        public void cadastrar(Clientes cliente);
        public void AtualizarCliente(Clientes clientes);
    }
}
