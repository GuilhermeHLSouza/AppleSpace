using Applespace.Models;

public interface ICompraRepositorio
{
    void RegistroEndereco(int idCliente, int cep, int numero, string rua, string bairro, string complemento);
    void SelectUsuario(Clientes cliente);
    void Venda(Pedido pedido);
}
