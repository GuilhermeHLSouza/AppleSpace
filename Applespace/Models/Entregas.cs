namespace Applespace.Models
{
    public class Entregas
    {
        public int IdEntrega { get; set; }
        public string Status { get; set; }
        public int IdTransportadora { get; set; }
        public int IdEndereco { get; set; }
        public int IdPedido { get; set; }

        public Pedido Pedido { get; set; }
        public Enderecos Endereco { get; set; }
        public Transportadoras Transportadora { get; set; }
    }
}