using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrailerBoard.Application.Favorites.Commands.AddFavorite;
using TrailerBoard.Application.Favorites.Commands.RemoveFavorite;
using TrailerBoard.Application.Favorites.Queries.ListFavorites;
using TrailerBoard.Contracts;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class FavoriteController : ControllerBase
{
    private readonly IMediator _mediator;
    public FavoriteController(IMediator mediator) => _mediator = mediator;

    private string GetEmail() =>
        User.FindFirst(ClaimTypes.Email)?.Value
        ?? throw new UnauthorizedAccessException("email_not_found");

    [HttpPost]
    public async Task<ActionResult<FavoriteDto>> Add(AddFavoriteCommand command, CancellationToken ct)
    {
        var email = GetEmail();
        var result = await _mediator.Send(command with { Email = email }, ct);
        return Created($"/api/favorite/{result.PublicId}", result);
    }


    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<FavoriteDto>>> List(CancellationToken ct)
    {
        var email = GetEmail();
        var result = await _mediator.Send(new ListFavoritesQuery(email), ct);
        return Ok(result);
    }

    [HttpDelete("{publicId}")]
    public async Task<IActionResult> Delete(string publicId, CancellationToken ct)
    {
        var email = GetEmail();
        await _mediator.Send(new RemoveFavoriteCommand(email, publicId), ct);
        return NoContent();
    }
}
