using App;
using App.Services;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using Moq;

namespace AppTests;

public class AuthorizationServiceNegativeTests
{
	[Fact]
	public async Task LoginAsync_Throws_WhenUserNotFound()
	{
		var repo = new Mock<IUserRepository>();
		repo.Setup(x => x.GetByEmailAsync(It.IsAny<Email>()))
			.ReturnsAsync((User?)null);

		var uow = new Mock<IUnitOfWork>();
		uow.SetupGet(x => x.UserRepository).Returns(repo.Object);

		var service = new AuthorizationService(uow.Object);

		await Assert.ThrowsAsync<AuthorizationException>(() =>
			service.LoginAsync("test@mail.com", "123"));
	}

	[Fact]
	public async Task LoginAsync_Throws_WhenPasswordInvalid()
	{
		var user = TestData.Admin();

		var repo = new Mock<IUserRepository>();
		repo.Setup(x => x.GetByEmailAsync(user.Email)).ReturnsAsync(user);

		var uow = new Mock<IUnitOfWork>();
		uow.SetupGet(x => x.UserRepository).Returns(repo.Object);

		var service = new AuthorizationService(uow.Object);

		await Assert.ThrowsAsync<AuthorizationException>(() =>
			service.LoginAsync(user.Email, "wrong"));
	}
}
