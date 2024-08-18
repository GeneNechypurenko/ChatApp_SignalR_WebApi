
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

			builder.Services.AddScoped<IChatService<UserDTO>, UserService>();
			builder.Services.AddScoped<IChatService<ChatMessageDTO>, ChatMessageService>();
			builder.Services.AddScoped<IChatService<UserChatSessionDTO>, UserChatSessionService>();

			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
