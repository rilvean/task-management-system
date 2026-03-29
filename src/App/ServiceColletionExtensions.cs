using App.Services;
using Microsoft.Extensions.DependencyInjection;

namespace App;

public static class ServiceColletionExtensions
{
	public static IServiceCollection AddApp(this IServiceCollection services)
	{
		services.AddScoped<AuthorizationService>();
		services.AddScoped<WorkTaskService>();
		services.AddScoped<UserService>();

		return services;
	}
}