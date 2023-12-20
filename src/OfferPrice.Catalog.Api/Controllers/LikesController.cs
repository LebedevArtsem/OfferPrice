using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OfferPrice.Catalog.Api.Models;
using OfferPrice.Catalog.Domain;

namespace OfferPrice.Catalog.Api.Controllers;

[ApiController]
[Route("api/products/{productId}/likes")]
public class LikesController : ControllerBase //todo
{
    private readonly ILikeRepository _likes;

    private readonly IMapper _mapper;

    public LikesController(ILikeRepository likes, IMapper mapper)
    {
        _likes = likes;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetCountsOfLikes([FromRoute] string productId, CancellationToken token)
    {
        var count = await _likes.GetCount(productId, token);

        return Ok(count);
    }

    [HttpPost]
    public async Task<IActionResult> CreateLike(string productId, LikeRequest likeRequest, CancellationToken token)
    {
        var like = new Domain.Like
        {
            ProductId = productId,
            UserId = likeRequest.UserId
        };

        await _likes.Create(like, token);

        return Ok(like);
    }

    [HttpDelete("for/{userId}")]
    public async Task<IActionResult> DeleteLike([FromRoute] string productId, [FromRoute] string userId, CancellationToken token)
    {
        await _likes.Delete(productId, userId, token);

        return Ok();
    }

    [HttpGet("by/{userId}")]
    public async Task<IActionResult> GetLike([FromRoute] string productId, [FromRoute] string userId, CancellationToken token)
    {
        var like = await _likes.Get(productId, userId, token);

        if (like == null)
        {
            return NotFound();
        }

        var likeResponse = _mapper.Map<Models.Like>(like);

        return Ok(likeResponse);
    }
}

