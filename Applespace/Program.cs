using Applespace.Libraries.LoginClientes;
using Applespace.Libraries.Sessao;
using Applespace.Repositorio.Carrinho;
using Applespace.Repositorio.Login;
using Applespace.Repositorio.Produto;
using Applespace.Data;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

// Repositórios e serviços do sistema
builder.Services.AddScoped<IProdutoRepositorio, ProdutoRepositorio>();
builder.Services.AddScoped<ILoginRepositorio, LoginRepositorio>();
builder.Services.AddScoped<ICarrinhoRepositorio, CarrinhoRepositorio>();

// Registro do banco de dados (IMPORTANTE: antes do builder.Build())
builder.Services.AddTransient<Database>();

// Sessão e login de clientes
builder.Services.AddSingleton<Sessao, Sessao>();
builder.Services.AddScoped<LoginClientes, LoginClientes>();

builder.Services.AddSession();

// Cria o app depois de todos os serviços registrados
var app = builder.Build();

// Configuração do pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthorization();

// Rota padrão
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Aplica a cultura pt-BR globalmente
var cultureInfo = new CultureInfo("pt-BR");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

app.Run();
