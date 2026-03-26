using App.Services;
using Domain.Enums;
using App.Interfaces;
using Domain.Models;
using Moq;
using App.Enums;

namespace AppTests;

public class WorkTaskServiceTests
{
	[Fact]
	public async Task CreateAsync_AddsTask_AndSaves()
	{
		var manager = TestData.Manager();

		var userRepo = new Mock<IUserRepository>();
		userRepo.Setup(x => x.GetByIdAsync(manager.Id)).ReturnsAsync(manager);

		var taskRepo = new Mock<IWorkTaskRepository>();

		var uow = new Mock<IUnitOfWork>();
		uow.SetupGet(x => x.UserRepository).Returns(userRepo.Object);
		uow.SetupGet(x => x.WorkTaskRepository).Returns(taskRepo.Object);

		var service = new WorkTaskService(uow.Object);

		var id = await service.CreateAsync(manager.Id, "task", null);

		Assert.NotEqual(Guid.Empty, id);
		taskRepo.Verify(x => x.AddAsync(It.Is<WorkTask>(t => t.Id == id)), Times.Once);
		uow.Verify(x => x.SaveChangesAsync(default), Times.Once);
	}

	[Fact]
	public async Task GetPerEmployeeAsync_ReturnsEmployeeTasks()
	{
		var employee = TestData.Employee();
		var task = TestData.Task();
		task.AssignExecutor(employee);

		var userRepo = new Mock<IUserRepository>();
		userRepo.Setup(x => x.GetByIdAsync(employee.Id)).ReturnsAsync(employee);

		var uow = new Mock<IUnitOfWork>();
		uow.SetupGet(x => x.UserRepository).Returns(userRepo.Object);

		var service = new WorkTaskService(uow.Object);

		var tasks = await service.GetPerEmployeeAsync(employee.Id);

		Assert.Single(tasks);
		Assert.Equal(task.Id, tasks[0].Id);
	}

	[Fact]
	public async Task GetAllSortedAsync_ReturnsRepositoryResult()
	{
		var manager = TestData.Manager();
		var expected = new List<WorkTask> { TestData.Task() };

		var userRepo = new Mock<IUserRepository>();
		userRepo.Setup(x => x.GetByIdAsync(manager.Id)).ReturnsAsync(manager);

		var taskRepo = new Mock<IWorkTaskRepository>();
		taskRepo.Setup(x => x.GetAllSortedAsync(WorkTaskSortBy.Name, true)).ReturnsAsync(expected);

		var uow = new Mock<IUnitOfWork>();
		uow.SetupGet(x => x.UserRepository).Returns(userRepo.Object);
		uow.SetupGet(x => x.WorkTaskRepository).Returns(taskRepo.Object);

		var service = new WorkTaskService(uow.Object);

		var tasks = await service.GetAllSortedAsync(manager.Id, WorkTaskSortBy.Name, true);

		Assert.Equal(expected, tasks);
	}

	[Fact]
	public async Task GetByPriorityAsync_ReturnsRepositoryResult()
	{
		var manager = TestData.Manager();
		var expected = new List<WorkTask> { TestData.Task() };

		var userRepo = new Mock<IUserRepository>();
		userRepo.Setup(x => x.GetByIdAsync(manager.Id)).ReturnsAsync(manager);

		var taskRepo = new Mock<IWorkTaskRepository>();
		taskRepo.Setup(x => x.GetByPriorityAsync(WorkTaskPriority.High)).ReturnsAsync(expected);

		var uow = new Mock<IUnitOfWork>();
		uow.SetupGet(x => x.UserRepository).Returns(userRepo.Object);
		uow.SetupGet(x => x.WorkTaskRepository).Returns(taskRepo.Object);

		var service = new WorkTaskService(uow.Object);

		var tasks = await service.GetByPriorityAsync(manager.Id, WorkTaskPriority.High);

		Assert.Equal(expected, tasks);
	}

	[Fact]
	public async Task GetByStatusAsync_ReturnsRepositoryResult()
	{
		var manager = TestData.Manager();
		var expected = new List<WorkTask> { TestData.Task() };

		var userRepo = new Mock<IUserRepository>();
		userRepo.Setup(x => x.GetByIdAsync(manager.Id)).ReturnsAsync(manager);

		var taskRepo = new Mock<IWorkTaskRepository>();
		taskRepo.Setup(x => x.GetByStatusAsync(WorkTaskStatus.Active)).ReturnsAsync(expected);

		var uow = new Mock<IUnitOfWork>();
		uow.SetupGet(x => x.UserRepository).Returns(userRepo.Object);
		uow.SetupGet(x => x.WorkTaskRepository).Returns(taskRepo.Object);

		var service = new WorkTaskService(uow.Object);

		var tasks = await service.GetByStatusAsync(manager.Id, WorkTaskStatus.Active);

		Assert.Equal(expected, tasks);
	}

	[Fact]
	public async Task AssignExecutorAsync_AssignsExecutor_AndSaves()
	{
		var manager = TestData.Manager();
		var employee = TestData.Employee();
		var task = TestData.Task();

		var userRepo = new Mock<IUserRepository>();
		userRepo.Setup(x => x.GetByIdAsync(manager.Id)).ReturnsAsync(manager);
		userRepo.Setup(x => x.GetByIdAsync(employee.Id)).ReturnsAsync(employee);

		var taskRepo = new Mock<IWorkTaskRepository>();
		taskRepo.Setup(x => x.GetByIdAsync(task.Id)).ReturnsAsync(task);

		var uow = new Mock<IUnitOfWork>();
		uow.SetupGet(x => x.UserRepository).Returns(userRepo.Object);
		uow.SetupGet(x => x.WorkTaskRepository).Returns(taskRepo.Object);

		var service = new WorkTaskService(uow.Object);

		await service.AssignExecutorAsync(manager.Id, task.Id, employee.Id);

		Assert.Contains(task.Executors, u => u.Id == employee.Id);
		uow.Verify(x => x.SaveChangesAsync(default), Times.Once);
	}

	[Fact]
	public async Task RemoveExecutorAsync_RemovesExecutor_AndSaves()
	{
		var manager = TestData.Manager();
		var employee = TestData.Employee();
		var task = TestData.Task();
		task.AssignExecutor(employee);

		var userRepo = new Mock<IUserRepository>();
		userRepo.Setup(x => x.GetByIdAsync(manager.Id)).ReturnsAsync(manager);
		userRepo.Setup(x => x.GetByIdAsync(employee.Id)).ReturnsAsync(employee);

		var taskRepo = new Mock<IWorkTaskRepository>();
		taskRepo.Setup(x => x.GetByIdAsync(task.Id)).ReturnsAsync(task);

		var uow = new Mock<IUnitOfWork>();
		uow.SetupGet(x => x.UserRepository).Returns(userRepo.Object);
		uow.SetupGet(x => x.WorkTaskRepository).Returns(taskRepo.Object);

		var service = new WorkTaskService(uow.Object);

		await service.RemoveExecutorAsync(manager.Id, task.Id, employee.Id);

		Assert.DoesNotContain(task.Executors, u => u.Id == employee.Id);
		uow.Verify(x => x.SaveChangesAsync(default), Times.Once);
	}

	[Fact]
	public async Task ChangePriorityAsync_ChangesPriority_AndSaves()
	{
		var manager = TestData.Manager();
		var task = TestData.Task();

		var userRepo = new Mock<IUserRepository>();
		userRepo.Setup(x => x.GetByIdAsync(manager.Id)).ReturnsAsync(manager);

		var taskRepo = new Mock<IWorkTaskRepository>();
		taskRepo.Setup(x => x.GetByIdAsync(task.Id)).ReturnsAsync(task);

		var uow = new Mock<IUnitOfWork>();
		uow.SetupGet(x => x.UserRepository).Returns(userRepo.Object);
		uow.SetupGet(x => x.WorkTaskRepository).Returns(taskRepo.Object);

		var service = new WorkTaskService(uow.Object);

		await service.ChangePriorityAsync(manager.Id, task.Id, WorkTaskPriority.High);

		Assert.Equal(WorkTaskPriority.High, task.Priority);
		uow.Verify(x => x.SaveChangesAsync(default), Times.Once);
	}

	[Fact]
	public async Task ChangeStatusAsync_ChangesStatus_AndSaves()
	{
		var manager = TestData.Manager();
		var task = TestData.Task();

		var userRepo = new Mock<IUserRepository>();
		userRepo.Setup(x => x.GetByIdAsync(manager.Id)).ReturnsAsync(manager);

		var taskRepo = new Mock<IWorkTaskRepository>();
		taskRepo.Setup(x => x.GetByIdAsync(task.Id)).ReturnsAsync(task);

		var uow = new Mock<IUnitOfWork>();
		uow.SetupGet(x => x.UserRepository).Returns(userRepo.Object);
		uow.SetupGet(x => x.WorkTaskRepository).Returns(taskRepo.Object);

		var service = new WorkTaskService(uow.Object);

		await service.ChangeStatusAsync(manager.Id, task.Id, WorkTaskStatus.Active);

		Assert.Equal(WorkTaskStatus.Active, task.Status);
		uow.Verify(x => x.SaveChangesAsync(default), Times.Once);
	}

	[Fact]
	public async Task RenameAsync_ChangesName_AndSaves()
	{
		var manager = TestData.Manager();
		var task = TestData.Task();

		var userRepo = new Mock<IUserRepository>();
		userRepo.Setup(x => x.GetByIdAsync(manager.Id)).ReturnsAsync(manager);

		var taskRepo = new Mock<IWorkTaskRepository>();
		taskRepo.Setup(x => x.GetByIdAsync(task.Id)).ReturnsAsync(task);

		var uow = new Mock<IUnitOfWork>();
		uow.SetupGet(x => x.UserRepository).Returns(userRepo.Object);
		uow.SetupGet(x => x.WorkTaskRepository).Returns(taskRepo.Object);

		var service = new WorkTaskService(uow.Object);

		await service.RenameAsync(manager.Id, task.Id, "new name");

		Assert.Equal("new name", task.Name);
		uow.Verify(x => x.SaveChangesAsync(default), Times.Once);
	}

	[Fact]
	public async Task ChangeDescriptionAsync_ChangesDescription_AndSaves()
	{
		var manager = TestData.Manager();
		var task = TestData.Task();

		var userRepo = new Mock<IUserRepository>();
		userRepo.Setup(x => x.GetByIdAsync(manager.Id)).ReturnsAsync(manager);

		var taskRepo = new Mock<IWorkTaskRepository>();
		taskRepo.Setup(x => x.GetByIdAsync(task.Id)).ReturnsAsync(task);

		var uow = new Mock<IUnitOfWork>();
		uow.SetupGet(x => x.UserRepository).Returns(userRepo.Object);
		uow.SetupGet(x => x.WorkTaskRepository).Returns(taskRepo.Object);

		var service = new WorkTaskService(uow.Object);

		await service.ChangeDescriptionAsync(manager.Id, task.Id, "description");

		Assert.Equal("description", task.Description);
		uow.Verify(x => x.SaveChangesAsync(default), Times.Once);
	}

	[Fact]
	public async Task SetDeadlineAsync_ChangesDeadline_AndSaves()
	{
		var manager = TestData.Manager();
		var task = TestData.Task();
		var deadline = DateTimeOffset.UtcNow.AddDays(2);

		var userRepo = new Mock<IUserRepository>();
		userRepo.Setup(x => x.GetByIdAsync(manager.Id)).ReturnsAsync(manager);

		var taskRepo = new Mock<IWorkTaskRepository>();
		taskRepo.Setup(x => x.GetByIdAsync(task.Id)).ReturnsAsync(task);

		var uow = new Mock<IUnitOfWork>();
		uow.SetupGet(x => x.UserRepository).Returns(userRepo.Object);
		uow.SetupGet(x => x.WorkTaskRepository).Returns(taskRepo.Object);

		var service = new WorkTaskService(uow.Object);

		await service.SetDeadlineAsync(manager.Id, task.Id, deadline);

		Assert.Equal(deadline, task.Deadline);
		uow.Verify(x => x.SaveChangesAsync(default), Times.Once);
	}

	[Fact]
	public async Task CompleteByAsync_CompletesTask_AndSaves()
	{
		var employee = TestData.Employee();
		var task = TestData.Task();
		task.AssignExecutor(employee);

		var userRepo = new Mock<IUserRepository>();
		userRepo.Setup(x => x.GetByIdAsync(employee.Id)).ReturnsAsync(employee);

		var taskRepo = new Mock<IWorkTaskRepository>();
		taskRepo.Setup(x => x.GetByIdAsync(task.Id)).ReturnsAsync(task);

		var uow = new Mock<IUnitOfWork>();
		uow.SetupGet(x => x.UserRepository).Returns(userRepo.Object);
		uow.SetupGet(x => x.WorkTaskRepository).Returns(taskRepo.Object);

		var service = new WorkTaskService(uow.Object);

		await service.CompleteByAsync(employee.Id, task.Id);

		Assert.Equal(WorkTaskStatus.Completed, task.Status);
		uow.Verify(x => x.SaveChangesAsync(default), Times.Once);
	}

	[Fact]
	public async Task DeleteAsync_DeletesTask_AndSaves()
	{
		var manager = TestData.Manager();
		var task = TestData.Task();

		var userRepo = new Mock<IUserRepository>();
		userRepo.Setup(x => x.GetByIdAsync(manager.Id)).ReturnsAsync(manager);

		var taskRepo = new Mock<IWorkTaskRepository>();
		taskRepo.Setup(x => x.GetByIdAsync(task.Id)).ReturnsAsync(task);

		var uow = new Mock<IUnitOfWork>();
		uow.SetupGet(x => x.UserRepository).Returns(userRepo.Object);
		uow.SetupGet(x => x.WorkTaskRepository).Returns(taskRepo.Object);

		var service = new WorkTaskService(uow.Object);

		await service.DeleteAsync(manager.Id, task.Id);

		taskRepo.Verify(x => x.Delete(task), Times.Once);
		uow.Verify(x => x.SaveChangesAsync(default), Times.Once);
	}
}