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
	public async Task CreateAsync_AddsUser_AndReturnsId()
	{
		var admin = TestData.Admin();

		var repo = new Mock<IUserRepository>();
		repo.Setup(x => x.GetByIdAsync(admin.Id)).ReturnsAsync(admin);

		var uow = new Mock<IUnitOfWork>();
		uow.SetupGet(x => x.UserRepository).Returns(repo.Object);

		var service = new UserService(uow.Object);

		var id = await service.CreateAsync(
			admin.Id,
			"user",
			Email.From("test@mail.com"),
			PasswordHasher.Hash("123"),
			UserRole.Employee);

		Assert.NotEqual(Guid.Empty, id);
		repo.Verify(x => x.AddAsync(It.Is<User>(u => u.Id == id)), Times.Once);
		uow.Verify(x => x.SaveChangesAsync(default), Times.Once);
	}

	[Fact]
	public async Task GetAllSortedAsync_ReturnsRepositoryResult()
	{
		var admin = TestData.Admin();
		var expected = new List<User> { TestData.Employee() };

		var repo = new Mock<IUserRepository>();
		repo.Setup(x => x.GetByIdAsync(admin.Id)).ReturnsAsync(admin);
		repo.Setup(x => x.GetAllSortedAsync(UserSortBy.Name, true)).ReturnsAsync(expected);

		var uow = new Mock<IUnitOfWork>();
		uow.SetupGet(x => x.UserRepository).Returns(repo.Object);

		var service = new UserService(uow.Object);

		var users = await service.GetAllSortedAsync(admin.Id, UserSortBy.Name, descending: true);

		Assert.Equal(expected, users);
	}

	[Fact]
	public async Task GetPerTaskAsync_ReturnsTaskExecutors()
	{
		var manager = TestData.Manager();
		var task = TestData.Task();
		var employee = TestData.Employee();
		task.AssignExecutor(employee);

		var userRepo = new Mock<IUserRepository>();
		userRepo.Setup(x => x.GetByIdAsync(manager.Id)).ReturnsAsync(manager);

		var taskRepo = new Mock<IWorkTaskRepository>();
		taskRepo.Setup(x => x.GetByIdAsync(task.Id)).ReturnsAsync(task);

		var uow = new Mock<IUnitOfWork>();
		uow.SetupGet(x => x.UserRepository).Returns(userRepo.Object);
		uow.SetupGet(x => x.WorkTaskRepository).Returns(taskRepo.Object);

		var service = new UserService(uow.Object);

		var executors = await service.GetPerTaskAsync(manager.Id, task.Id);

		Assert.Single(executors);
		Assert.Equal(employee.Id, executors[0].Id);
	}

	[Fact]
	public async Task DeleteAsync_RemovesUser_AndSaves()
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
		uow.Verify(x => x.SaveChangesAsync(default), Times.Once);
	}

	[Fact]
	public async Task RenameAsync_ChangesName_AndSaves()
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
		uow.Verify(x => x.SaveChangesAsync(default), Times.Once);
	}

	[Fact]
	public async Task ChangeEmailAsync_ChangesEmail_AndSaves()
	{
		var admin = TestData.Admin();
		var user = TestData.Employee();
		var newEmail = Email.From("new@mail.com");

		var repo = new Mock<IUserRepository>();
		repo.Setup(x => x.GetByIdAsync(admin.Id)).ReturnsAsync(admin);
		repo.Setup(x => x.GetByIdAsync(user.Id)).ReturnsAsync(user);

		var uow = new Mock<IUnitOfWork>();
		uow.SetupGet(x => x.UserRepository).Returns(repo.Object);

		var service = new UserService(uow.Object);

		await service.ChangeEmailAsync(admin.Id, user.Id, newEmail);

		Assert.Equal(newEmail, user.Email);
		uow.Verify(x => x.SaveChangesAsync(default), Times.Once);
	}

	[Fact]
	public async Task ChangePasswordAsync_ChangesPassword_AndSaves()
	{
		var admin = TestData.Admin();
		var user = TestData.Employee();
		var newPassword = PasswordHasher.Hash("new-password");

		var repo = new Mock<IUserRepository>();
		repo.Setup(x => x.GetByIdAsync(admin.Id)).ReturnsAsync(admin);
		repo.Setup(x => x.GetByIdAsync(user.Id)).ReturnsAsync(user);

		var uow = new Mock<IUnitOfWork>();
		uow.SetupGet(x => x.UserRepository).Returns(repo.Object);

		var service = new UserService(uow.Object);

		await service.ChangePasswordAsync(admin.Id, user.Id, newPassword);

		Assert.Equal(newPassword, user.PasswordHash);
		uow.Verify(x => x.SaveChangesAsync(default), Times.Once);
	}

	[Fact]
	public async Task ChangeRoleAsync_ChangesRole_AndSaves()
	{
		var admin = TestData.Admin();
		var user = TestData.Employee();

		var repo = new Mock<IUserRepository>();
		repo.Setup(x => x.GetByIdAsync(admin.Id)).ReturnsAsync(admin);
		repo.Setup(x => x.GetByIdAsync(user.Id)).ReturnsAsync(user);

		var uow = new Mock<IUnitOfWork>();
		uow.SetupGet(x => x.UserRepository).Returns(repo.Object);

		var service = new UserService(uow.Object);

		await service.ChangeRoleAsync(admin.Id, user.Id, UserRole.Manager);

		Assert.Equal(UserRole.Manager, user.Role);
		uow.Verify(x => x.SaveChangesAsync(default), Times.Once);
	}
}

