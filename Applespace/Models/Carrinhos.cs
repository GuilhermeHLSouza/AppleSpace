namespace Applespace.Models
{
    public class Carrinhos
    {
        public int idCarrinho { get; set; }
        public int quantidade {  get; set; }
        public double preco {  get; set; }
        public int codBarra {  get; set; }
        public Produtos produtos { get; set; }

        public int idCliente { get; set; }
        public Clientes clientes { get; set; }

    }
}
