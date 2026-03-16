using App;
using App.Services;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Services;
using Domain.ValueObjects;
using Moq;

namespace AppTests;

public class UserServiceNegativeTests
{
	[Fact]
	public async Task CreateAsync_Throws_WhenNotAdmin()
	{
		var employee = TestData.Employee();

		var repo = new Mock<IUserRepository>();
		repo.Setup(x => x.GetByIdAsync(employee.Id))
			.ReturnsAsync(employee);

		var uow = new Mock<IUnitOfWork>();
		uow.SetupGet(x => x.UserRepository).Returns(repo.Object);

		var service = new UserService(uow.Object);

		await Assert.ThrowsAsync<AccessException>(() =>
			service.CreateAsync(
				employee.Id,
				"user",
				Email.From("test@mail.com"),
				PasswordHasher.Hash("123"),
				UserRole.Employee));
	}
}