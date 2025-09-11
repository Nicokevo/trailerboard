namespace TrailerBoard.Contracts;

public record UserDto(
    string Email,
    string FirstName,
    string LastName,
    bool IsActive
);
