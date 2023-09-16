using MercadoIT.Web.DataAccess.Interfaces;
using MercadoIT.Web.DataAccess.Services;
using MercadoIT.Web.Entities;
using Microsoft.EntityFrameworkCore;

namespace MercadoIT.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<NorthwindContext>(opciones =>
                opciones.UseSqlServer("name=NorthwindDb"));

            builder.Services.AddScoped<IRepositoryAsync<Product>, RepositoryAsync<Product>>();
            builder.Services.AddScoped<IRepositoryAsync<Category>, RepositoryAsync<Category>>();
            builder.Services.AddScoped<IRepositoryAsync<Supplier>, RepositoryAsync<Supplier>>();
            builder.Services.AddScoped<IRepositoryAsync<Employee>, RepositoryAsync<Employee>>();

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