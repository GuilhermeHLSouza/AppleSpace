using Applespace.Models;

namespace Applespace.Repositorio.Login
{
    public interface ILoginRepositorio
    {
        Clientes Login(string email, string cpf, string senha);
        void Cadastrar(Clientes cliente);
        void AtualizarCliente(Clientes cliente);
        Clientes BuscarPorEmail(string email); // Opcional, mas recomendado
    }
}
