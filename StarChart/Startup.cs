using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using StarChart.Data;
using Microsoft.EntityFrameworkCore;

namespace StarChart
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Method (AddMvc) call on services to add support for later use. 
            services.AddMvc();
            // Method call to store for database usage. 
            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("StarChart"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Calls the 'UseMvc' method on app.
            app.UseMvc();
        }
    }
}
