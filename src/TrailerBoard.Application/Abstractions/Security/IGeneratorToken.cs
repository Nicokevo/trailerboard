using TrailerBoard.Domain;

namespace TrailerBoard.Application.Abstractions.Security;

public interface ITokenGenerator
{
    string Generate(User user);
}
