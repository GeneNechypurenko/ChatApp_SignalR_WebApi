
using BLL.Infrastructure;
using BLL.ModelsDTO;
using BLL.Services;
using BLL.Services.Interfaces;

namespace ChatApp_SignalR_WebApi
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddApplicationDbContext(builder.Configuration.GetConnectionString("DefaultConnection")!);
			builder.Services.AddUnitOfWorkService();
			builder.Services.AddScoped<IUserService, UserService>();

			builder.Services.AddControllers();

			var app = builder.Build();

			app.UseStaticFiles();
			app.UseHttpsRedirection();
			app.MapControllers();

			app.Run();
		}
	}
}
