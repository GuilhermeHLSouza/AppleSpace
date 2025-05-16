namespace Applespace.Models
{
    public class Entregas
    {
        public int idEntrega { get; set; }
        public string status { get; set; }
        public int idTransp {  get; set; }
        public int idEndereco {  get; set; }
        public int idVenda {  get; set; }

        public Vendas Vendas { get; set; }
        public Enderecos Enderecos { get; set; }
        public Transportadoras Transportadoras { get; set; }

    }
}
