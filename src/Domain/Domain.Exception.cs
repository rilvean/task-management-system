namespace Domain.Exceptions;

public class EmailException(string message) : Exception(message) { }
public class PasswordHashException(string message) : Exception(message) { }
public class PasswordHasherException(string message) : Exception(message) { }
public class RepeatException(string message) : Exception(message) { }
public class NotFoundException(string message) : Exception(message) { }
