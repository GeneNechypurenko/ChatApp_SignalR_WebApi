using DAL.Repositories;
using DAL.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Infrastructure
{
	public static class UnitOfWorkServiceExtensions
	{
		public static void AddUnitOfWorkService(this IServiceCollection services) => services.AddScoped<IUnitOfWork, UnitOfWork>();
	}
}
