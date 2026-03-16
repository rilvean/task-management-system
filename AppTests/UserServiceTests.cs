using App.Services;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Models;
using Domain.Services;
using Domain.ValueObjects;
using Moq;

namespace AppTests;

public class UserServiceTests
{
	[Fact]
	public async Task CreateAsync_AddsUser()
	{
		var admin = TestData.Admin();

		var repo = new Mock<IUserRepository>();
		repo.Setup(x => x.GetByIdAsync(admin.Id))
			.ReturnsAsync(admin);

		var uow = new Mock<IUnitOfWork>();
		uow.SetupGet(x => x.UserRepository).Returns(repo.Object);

		var service = new UserService(uow.Object);

		await service.CreateAsync(
			admin.Id,
			"user",
			Email.From("test@meail.com"),
			PasswordHasher.Hash("123"),
			UserRole.Employee);

		repo.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
		uow.Verify(x => x.SaveChangesAsync(default), Times.Once);
	}

	[Fact]
	public async Task DeleteAsync_RemovesUser()
	{
		var admin = TestData.Admin();
		var user = TestData.Employee();

		var repo = new Mock<IUserRepository>();
		repo.Setup(x => x.GetByIdAsync(admin.Id)).ReturnsAsync(admin);
		repo.Setup(x => x.GetByIdAsync(user.Id)).ReturnsAsync(user);

		var uow = new Mock<IUnitOfWork>();
		uow.SetupGet(x => x.UserRepository).Returns(repo.Object);

		var service = new UserService(uow.Object);

		await service.DeleteAsync(admin.Id, user.Id);

		repo.Verify(x => x.Delete(user), Times.Once);
	}

	[Fact]
	public async Task RenameAsync_ChangesName()
	{
		var admin = TestData.Admin();
		var user = TestData.Employee();

		var repo = new Mock<IUserRepository>();
		repo.Setup(x => x.GetByIdAsync(admin.Id)).ReturnsAsync(admin);
		repo.Setup(x => x.GetByIdAsync(user.Id)).ReturnsAsync(user);

		var uow = new Mock<IUnitOfWork>();
		uow.SetupGet(x => x.UserRepository).Returns(repo.Object);

		var service = new UserService(uow.Object);

		await service.RenameAsync(admin.Id, user.Id, "new");

		Assert.Equal("new", user.Name);
	}
}