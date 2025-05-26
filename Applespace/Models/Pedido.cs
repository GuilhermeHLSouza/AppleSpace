namespace Applespace.Models
{
    public class Pedido
    {
        public int IdPedido { get; set; }
        public string FormaPagamento { get; set; }
        public string Status { get; set; }
        public int IdCarrinho { get; set; }
    }
}
