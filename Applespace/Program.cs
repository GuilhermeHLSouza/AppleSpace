using Applespace.Data;
using Applespace.Libraries.LoginClientes;
using Applespace.Libraries.Sessao;
using Applespace.Repositorio.Carrinho;
using Applespace.Repositorio.Compra;
using Applespace.Repositorio.Login;
using Applespace.Repositorio.Produto;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Serviços MVC
builder.Services.AddControllersWithViews();

// Acesso ao HTTP Context
builder.Services.AddHttpContextAccessor();

// Registro dos Repositórios
builder.Services.AddScoped<IProdutoRepositorio, ProdutoRepositorio>();
builder.Services.AddScoped<ILoginRepositorio, LoginRepositorio>();
builder.Services.AddScoped<ICarrinhoRepositorio, CarrinhoRepositorio>();
builder.Services.AddScoped<ICompraRepositorio, CompraRepositorio>();

// Banco de Dados
builder.Services.AddTransient<Database>();

// Sessão e Login
builder.Services.AddSingleton<Sessao>();
builder.Services.AddScoped<LoginClientes>();

// Sessão
builder.Services.AddSession();

// Construção do app
var app = builder.Build();

// Configurações de ambiente
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

// Middleware padrão
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

// Definição de Rotas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

// Aplicando Cultura pt-BR globalmente
var defaultCulture = new CultureInfo("pt-BR");
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(defaultCulture),
    SupportedCultures = new List<CultureInfo> { defaultCulture },
    SupportedUICultures = new List<CultureInfo> { defaultCulture }
};
app.UseRequestLocalization(localizationOptions);

app.Run();
