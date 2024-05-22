using Microsoft.Extensions.DependencyInjection.Extensions;
using Muzej.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Muzej.Repository.Interfaces;
using Muzej.SqlServerRepository;

namespace Muzej.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // configure sql server
            string connectionString = builder.Configuration.GetConnectionString("MUZ");
            builder.Services.AddDbContext<MUZContext>(optionsBuilder
                => optionsBuilder.UseSqlServer(connectionString));


            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //define repository dependeny injection
            builder.Services.AddScoped<IRepositoryWrapper, SqlServerRepository.RepositoryWrapper>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}