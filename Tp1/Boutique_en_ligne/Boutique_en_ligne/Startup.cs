namespace Boutique_en_ligne
{
    using Microsoft.Extensions.DependencyInjection;

    namespace Boutique_en_ligne
    {
        public class Startup
        {
            public void ConfigureServices(IServiceCollection services)
            {
                ConfigureHttpClient(services);
                ConfigureMvc(services);
            }

            private void ConfigureHttpClient(IServiceCollection services)
            {
                services.AddHttpClient();
            }

            private void ConfigureMvc(IServiceCollection services)
            {
                services.AddControllersWithViews();
            }
        }
    }

}
