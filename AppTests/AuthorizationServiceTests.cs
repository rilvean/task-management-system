using App.Services;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Models;
using Domain.Services;
using Domain.ValueObjects;
using Moq;

namespace AppTests;

public class AuthorizationServiceTests
{
	[Fact]
	public async Task LoginAsync_ReturnsUserData_WhenCredentialsValid()
	{
		var user = TestData.Admin();
		var password = "password";

		var repo = new Mock<IUserRepository>();
		repo.Setup(x => x.GetByEmailAsync(user.Email)).ReturnsAsync(user);

		var uow = new Mock<IUnitOfWork>();
		uow.SetupGet(x => x.UserRepository).Returns(repo.Object);

		var service = new AuthorizationService(uow.Object);

		var result = await service.LoginAsync(user.Email, password);

		Assert.Equal(user.Id, result.id);
		Assert.Equal(user.Role, result.role);
		Assert.Equal(user.Name, result.name);
	}
}