using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Models.Submodels;
using Domain.ValueObjects;

namespace Domain.Models;


public class User : IAuditable
{
	#region Fields
	private const int NAME_MAX_LENGTH = 50;

	private string _name = null!;
	private Email _email = null!;
	private PasswordHash _password = null!;

	private readonly List<Assignment> _assignments = [];

	public Guid Id { get; private set; } = Guid.NewGuid();
	public string Name
	{
		get => _name;
		private set
		{
			if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(Name));
			if (value.Length > NAME_MAX_LENGTH) throw new ArgumentOutOfRangeException(nameof(Name));
			_name = value;
		}
	}
	public Email Email
	{
		get => _email;
		private set
		{
			if (value is null) throw new ArgumentNullException(nameof(Email));
			_email = value;
		}
	}
	public PasswordHash PasswordHash
	{
		get => _password;
		private set
		{
			if (value is null) throw new ArgumentNullException(nameof(PasswordHash));
			_password = value;
		}
	}
	public UserRole Role { get; private set; }

	public IEnumerable<WorkTask> Tasks => _assignments.Select(x => x.Task);
	#endregion

	private User() { }

	public User(string name, Email email, PasswordHash passwordHash, UserRole role)
	{
		Name = name;
		Email = email;
		PasswordHash = passwordHash;
		Role = role;
	}

	public void ChangeName(string newName) => Name = newName;
	public void ChangeEmail(Email newEmail) => Email = newEmail;
	public void ChangePassword(PasswordHash newPasswordHash) => PasswordHash = newPasswordHash;
	public void ChangeRole(UserRole newRole) => Role = newRole;

	#region Internal methods
	internal void AddAssignment(Assignment assignment)
	{
		if (assignment is null) throw new ArgumentNullException(nameof(assignment));

		if (!_assignments.Contains(assignment))
			_assignments.Add(assignment);
	}

	internal void RemoveAssignment(Assignment assignment)
	{
		if (assignment is null) throw new ArgumentNullException(nameof(assignment));

		if (!_assignments.Remove(assignment))
			throw new NotFoundException(nameof(assignment));
			
	}
	#endregion
}