using Applespace.Models;
using Newtonsoft.Json;

namespace Applespace.Libraries.LoginClientes
{
    public class LoginClientes
    {
        private readonly Sessao.Sessao _sessao;
        private const string chave = "usuarioLogado";

        public LoginClientes(Sessao.Sessao sessao)
        {
            _sessao = sessao;
        }

        public void Login(Clientes cliente)
        {
            string clienteJson = JsonConvert.SerializeObject(cliente);
            _sessao.Criar(chave, clienteJson);
        }

        public Clientes GetUsuario()
        {
            if (_sessao.Existe(chave))
            {
                string clienteJson = _sessao.Consultar(chave);
                return JsonConvert.DeserializeObject<Clientes>(clienteJson);
            }
            return null;
        }

        public void Logout()
        {
            _sessao.Remover(chave);
        }
    }
}
