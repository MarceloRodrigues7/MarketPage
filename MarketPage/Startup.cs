using MarketPage.Repository;
using MarketPage.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MarketPage
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IUsuarioRepository, UsuarioRepository>();
            services.AddSingleton<IFreteRepository, FreteRepository>();
            services.AddSingleton<ICategoriaRepository, CategoriaRepository>();
            services.AddSingleton<IItemRepository, ItemRepository>();
            services.AddSingleton<IImagemRepository, ImagemRepository>();
            services.AddSingleton<ICodPromocionalRepository, CodPromocionalRepository>();
            services.AddSingleton<IMessagesContatoRepository, MessagesContatoRepository>();
            services.AddSingleton<IEnderecoRepository, EnderecoRepository>();
            services.AddSingleton<IPedidoRepository, PedidoRepository>();
            services.AddSingleton<ICarrinhoRepository, CarrinhoRepository>();
            services.AddSingleton<IFormaPagamentoRepository, FormaPagamentoRepository>();

            services.AddControllersWithViews();

            services.AddMvcCore();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => options.LoginPath = "/Login/Index");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseRequestLocalization(CultureServices.SetLocazationOptions("pt-BR"));
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseCookiePolicy();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
