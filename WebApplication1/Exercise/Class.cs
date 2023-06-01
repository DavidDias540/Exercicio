
namespace WebApplication1.Exercise
{


    public class Class
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Configuração das rotas
            services.AddOData();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // ...

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.EnableDependencyInjection();
                endpoints.Select().Filter().OrderBy().Count().MaxTop(null);
            });

            // ...
        }
    }
}
