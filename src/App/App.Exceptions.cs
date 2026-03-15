namespace App;

public class AuthorizationException(string message) : Exception(message) { }
public class AccessException(string message) : Exception(message) { }
public class NotFoundException(string message) : Exception(message) { }
