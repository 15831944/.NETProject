using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using WebApplication.Models;
using WebApplication.DAO;
using WebApplication.Service.ServiceImpl;
using WebApplication.Service;

namespace WebApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TodoContext>(opt =>
                 opt.UseInMemoryDatabase("TodoList"));
            services.AddDbContext<InventoryContext>(opt =>
                 opt.UseSqlServer(
                     Configuration.GetConnectionString("DefaultConnection")
                 )
            );
            services.AddDbContext<ClassStudentContext>(opt =>
                 opt.UseSqlServer(
                     Configuration.GetConnectionString("DefaultConnection")
                 )
            );
            services.AddDbContext<StudentContext>(opt =>
                opt.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")
                )
            );
            services.AddScoped<StudentService, StudentServiceImpl>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
