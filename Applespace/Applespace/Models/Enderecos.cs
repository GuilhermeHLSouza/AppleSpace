namespace Applespace.Models
{
    public class Enderecos
    {
        public int IdEnde { get; set; }
        public int CEP { get; set; }
        public int numero { get; set; }
        public string rua { get; set; }
        public string Bairro { get; set; }
        public string complemento {  get; set; }
        public int IDCliente {  get; set; }
        public Clientes cliente { get; set; }
    }
}
