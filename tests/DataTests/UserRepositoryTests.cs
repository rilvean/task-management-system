using Data.Repositories;
using Domain.Enums;
using Domain.Models;
using Domain.Services;
using Domain.ValueObjects;

namespace DataTests;

public class UserRepositoryTests
{
	private string _name = "test";
	private Email _email = Email.From("test@mail.com");
	private PasswordHash _password = PasswordHash.From(PasswordHasher.Hash("123"));
	private UserRole _role = UserRole.Employee;

	[Fact]
	public async Task AddAsync_ShouldAddUser()
	{
		var context = DbContextFactory.Create();
		var repo = new UserRepository(context);

		var user = new User(_name, _email, _password, _role);

		await repo.AddAsync(user);
		await context.SaveChangesAsync();

		Assert.Single(context.Users);
	}

	[Fact]
	public async Task GetByEmailAsync_ShouldReturnUser()
	{
		var context = DbContextFactory.Create();
		var repo = new UserRepository(context);

		var email = _email;

		var user = new User(_name, _email, _password, _role);

		context.Users.Add(user);
		await context.SaveChangesAsync();

		var result = await repo.GetByEmailAsync(email);

		Assert.NotNull(result);
	}

	[Fact]
	public async Task GetAllAsync_ShouldSortByName()
	{
		var context = DbContextFactory.Create();
		var repo = new UserRepository(context);

		var u1 = new User("b", Email.From("btest@mail.com"), PasswordHasher.Hash("1"), UserRole.Employee);
		var u2 = new User("a", Email.From("atest@mail.com"), PasswordHasher.Hash("1"), UserRole.Employee);

		context.Users.AddRange(u1, u2);
		await context.SaveChangesAsync();

		var result = await repo.GetAllAsync(UserSortBy.Name);

		Assert.Equal("a", result.First().Name);
	}

	[Fact]
	public async Task Remove_ShouldDeleteUser()
	{
		var context = DbContextFactory.Create();
		var repo = new UserRepository(context);

		var user = new User(_name, _email, _password, _role);

		context.Users.Add(user);
		await context.SaveChangesAsync();

		repo.Remove(user);
		await context.SaveChangesAsync();

		Assert.Empty(context.Users);
	}
}