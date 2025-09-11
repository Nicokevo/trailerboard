namespace TrailerBoard.Contracts;

public record LoginRequest(string Email, string Password);
public record RegisterRequest(string Email, string Password, string ConfirmPassword, string FirstName, string LastName);
public record AuthResponse(string Token, string Email, string FirstName, string LastName);
