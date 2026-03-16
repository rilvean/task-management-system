using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Data;

public static class DependencyInjection
{
	public static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<AppDbContext>(builder =>
			builder.UseSqlServer(configuration.GetConnectionString("Default")));

		services.AddScoped<IUnitOfWork, UnitOfWork>();

		return services;
	}
}