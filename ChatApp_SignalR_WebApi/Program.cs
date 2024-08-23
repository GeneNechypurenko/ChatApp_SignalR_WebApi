using BLL.Infrastructure;
using BLL.Services;
using BLL.Services.Interfaces;
using ChatApp_SignalR_WebApi.Hubs;

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
            builder.Services.AddScoped<IChatMessageService, ChatMessageService>();

            builder.Services.AddControllers();
            builder.Services.AddSignalR();

            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.MapControllers();
            app.MapHub<ChatHub>("/chat");

            app.Run();
        }
    }
}
