using LoginUpLevel.DTOs;
using LoginUpLevel.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LoginUpLevel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController :ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }
        [HttpGet("product/{productId}")]
        public async Task<ActionResult<IEnumerable<CommentDTO>>> GetCommentByProductId (int productId)
        {
            try
            {
                var commentsDto = await _commentService.GetCommentsByProductIdAsync(productId);
                return Ok(commentsDto);
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("rating/{productId:int}/{rating:int}")]
        public async Task<ActionResult<IEnumerable<CommentDTO>>> GetCommentByRating([FromRoute] int productId, [FromRoute] int rating)
        {
            try
            {
                var commentsDto = await _commentService.GetCommentsByRatingAsync(productId, rating);
                return Ok(commentsDto);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> AddComment([FromBody] CommentDTO commentDto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userId, out var customerId))
                {
                    return BadRequest("Invalid customer ID in user claims.");
                }
                commentDto.CustomerId = customerId;
                await _commentService.AddCommentAsync(commentDto);
                return CreatedAtAction(nameof(GetCommentByProductId), new { productId = commentDto.ProductId }, commentDto);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error adding comment: {ex.Message}");
            }
        }

        [HttpPut]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UpdateComment([FromBody] CommentDTO commentDto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userId, out var customerId))
                {
                    return BadRequest("Invalid customer ID in user claims.");
                }
                await _commentService.UpdateCommentAsync(commentDto, customerId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating comment: {ex.Message}");
            }
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            try
            {
                var comment = await _commentService.GetCommentsByProductIdAsync(id);
                if (comment == null)
                {
                    return NotFound("Comment not found.");
                }
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userId, out var customerId))
                {
                    return BadRequest("Invalid customer ID in user claims.");
                }
                if (comment.Any(c => c.CustomerId != customerId))
                {
                    return Forbid("You can only delete your own comments.");
                }
                await _commentService.DeleteCommentAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting comment: {ex.Message}");
            }
        }
    }
}
