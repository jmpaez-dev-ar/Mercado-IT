using MercadoIT.Web.DataAccess.Interfaces;
using MercadoIT.Web.DataAccess.Services;
using MercadoIT.Web.Entities;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace MercadoIT.Web
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Log.Logger = new LoggerConfiguration()
			.MinimumLevel.Verbose()
			//.MinimumLevel.Information()
			.WriteTo.Console(theme: AnsiConsoleTheme.Code)
			.WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
			.CreateLogger();

			Log.Information("Inicio la App");
			Log.Debug("Starting Debug");
			Log.Warning("Starting Warning");
			Log.Error("Hubo un Error");


			var builder = WebApplication.CreateBuilder(args);

			//IConfiguration configuracion = new ConfigurationBuilder()
			//	.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
			//	.Build();
			//Log.Logger = new LoggerConfiguration()
			//	.ReadFrom.Configuration(configuracion)
			//	.CreateLogger();

			//builder.Host.UseSerilog();
			//builder.WebHost.UseSerilog();

			// Add services to the container.
			builder.Services.AddControllersWithViews();

			builder.Services.AddDbContext<NorthwindContext>(opciones =>
				opciones.UseSqlServer("name=NorthwindDb"));

			builder.Services.AddScoped<IRepositoryAsync<Customer>, RepositoryAsync<Customer>>();
			builder.Services.AddScoped<IRepositoryAsync<Product>, RepositoryAsync<Product>>();
			builder.Services.AddScoped<IRepositoryAsync<Category>, RepositoryAsync<Category>>();
			builder.Services.AddScoped<IRepositoryAsync<Supplier>, RepositoryAsync<Supplier>>();
			builder.Services.AddScoped<IRepositoryAsync<Employee>, RepositoryAsync<Employee>>();
			builder.Services.AddScoped<IRepositoryAsync<Region>, RepositoryAsync<Region>>();
			builder.Services.AddScoped<IRepositoryAsync<Shipper>, RepositoryAsync<Shipper>>();
			builder.Services.AddScoped<IRepositoryAsync<Territory>, RepositoryAsync<Territory>>();
			builder.Services.AddScoped<IRepositoryAsync<Order>, RepositoryAsync<Order>>();
			builder.Services.AddScoped<IRepositoryAsync<OrderDetail>, RepositoryAsync<OrderDetail>>();
			builder.Services.AddScoped<IRepositoryAsync<Invoice>, RepositoryAsync<Invoice>>();

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
				pattern: "{controller=Customers}/{action=Index}/{id?}");

			app.Run();
		}
	}
}