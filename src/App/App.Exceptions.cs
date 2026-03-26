namespace App.Excepions;

public class AuthorizationException(string message) : Exception(message) { }
public class AccessException(string message) : Exception(message) { }
public class AppException(string message) : Exception(message) { }