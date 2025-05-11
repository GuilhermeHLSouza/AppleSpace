using Applespace.Models;
using Newtonsoft.Json;
namespace Applespace.Libraries.LoginClientes
{
    public class LoginClientes
    {
        private string key = "Login.Cliente";
        private Sessao.Sessao _sessao;
        
        public LoginClientes(Sessao.Sessao sessao) 
        {
            _sessao = sessao;
        }

        public void Login(Clientes clientes)
        {
            string loginJSONString = JsonConvert.SerializeObject(clientes);
            _sessao.Gravar(key, loginJSONString);
        }

        public Clientes GetCliente()
        {

            if (_sessao.Existe(key))
            {
                string loginJSONString = _sessao.Consultar(key);
                return JsonConvert.DeserializeObject<Clientes>(loginJSONString);
            }
            else
            {
                return null;
            }
        }
        public void Logout()
        {
            _sessao.RemoverTodos();
        }
    }
}
