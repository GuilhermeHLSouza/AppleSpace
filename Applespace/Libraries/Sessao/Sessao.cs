using Org.BouncyCastle.Asn1.IsisMtt.X509;

namespace Applespace.Libraries.Sessao
{
    public class Sessao
    {

        IHttpContextAccessor _context;
        public Sessao(IHttpContextAccessor context)
        {
            _context = context;
        }

        public void Gravar(string key, string valor)
        {
            _context.HttpContext?.Session.SetString(key, valor);
        }

        public string Consultar(string Key)
        {
            return _context.HttpContext.Session.GetString(Key);
        }


        public bool Existe(string Key)
        {
            if (_context.HttpContext?.Session.GetString(Key) == null)
            {
                return false;
            }

            return true;
        }


        public void RemoverTodos()
        {
            _context.HttpContext?.Session.Clear();
        }
    }
}
