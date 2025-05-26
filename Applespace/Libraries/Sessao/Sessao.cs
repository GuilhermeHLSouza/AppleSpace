using Microsoft.AspNetCore.Http;

namespace Applespace.Libraries.Sessao
{
    public class Sessao
    {
        private readonly IHttpContextAccessor _context;

        public Sessao(IHttpContextAccessor context)
        {
            _context = context;
        }

        public void Criar(string chave, string valor)
        {
            _context.HttpContext.Session.SetString(chave, valor);
        }

        public void Remover(string chave)
        {
            _context.HttpContext.Session.Remove(chave);
        }

        public string Consultar(string chave)
        {
            return _context.HttpContext.Session.GetString(chave);
        }

        public bool Existe(string chave)
        {
            return _context.HttpContext.Session.GetString(chave) != null;
        }

        public void RemoverTodos()
        {
            _context.HttpContext.Session.Clear();
        }
    }
}
