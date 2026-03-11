using Data;
using Data.Repositories;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App;

public static class DependencyInjection
{
	public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddScoped<IUserRepository, UserRepository>();
		services.AddScoped<ITaskRepository, TaskRepository>();

		services.AddDbContext<AppDbContext>(builder =>
		{
			builder.UseSqlServer(configuration.GetConnectionString("Default"));
		});

		return services;
	}
}