using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OfferPrice.Auction.Api.Models.Comment;
using OfferPrice.Auction.Domain.Interfaces;
using OfferPrice.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OfferPrice.Auction.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/lots/{lotId}")]
public class CommentController : ControllerBase
{
    private readonly ICommentRepository _commentRepository;
    private readonly IMapper _mapper;

    public CommentController(ICommentRepository commentRepository, IMapper mapper)
    {
        _commentRepository = commentRepository;
        _mapper = mapper;
    }

    [HttpGet("comments")]
    public async Task<IActionResult> GetLotsComments(
        [FromRoute] string lotId,
        [FromQuery] FindCommentRequest findComment,
        CancellationToken token)
    {
        var comments = await _commentRepository.GetAuctionComments(lotId, findComment.ToQuery(), token);

        return Ok(new CommentResponse(comments));
    }

    [HttpPost("comments")]
    public async Task<IActionResult> CommentLot([FromRoute] string lotId, [FromBody] CommentRequest comment, CancellationToken cancellationToken)
    {
        var userId = ClaimValuesExtractionHelper.GetClientIdFromUserClaimIn(HttpContext);

        var commentEntity = _mapper.Map<Domain.Models.Comment>(comment);
        commentEntity.Id = Guid.NewGuid().ToString();
        commentEntity.UserId = userId;
        commentEntity.LotId = lotId;
        commentEntity.CreationDate = DateTime.UtcNow;

        await _commentRepository.AddComment(commentEntity, cancellationToken);

        return Ok();
    }

    [HttpPost("comments/{commentId}")]
    public async Task<IActionResult> AddSubComment(
        [FromRoute] string lotId,
        [FromRoute] string commentId,
        [FromBody] CommentRequest comment,
        CancellationToken cancellationToken)
    {
        var userId = ClaimValuesExtractionHelper.GetClientIdFromUserClaimIn(HttpContext);

        var subComment = _mapper.Map<Domain.Models.SubComment>(comment);
        subComment.UserId = userId;
        subComment.LotId = lotId;
        subComment.CreationDate = DateTime.UtcNow;

        await _commentRepository.AddSubComment(commentId, subComment, cancellationToken);

        return Ok();
    }
}

