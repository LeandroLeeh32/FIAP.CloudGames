using FIAP.CloudGames.API.Controllers.DTOs.Requests.Promotions;
using FIAP.CloudGames.API.Controllers.DTOs.Responses.Promotions;
using FIAP.CloudGames.Application.UseCases.Promotions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.CloudGames.API.Controllers
{
    /// <summary>
    /// Endpoints for managing promotions.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class PromotionsController : ControllerBase
    {
        private readonly GetPromotionsUseCase _getPromotionsUseCase;
        private readonly GetPromotionByIdUseCase _getPromotionByIdUseCase;
        private readonly CreatePromotionUseCase _createPromotionUseCase;
        private readonly UpdatePromotionUseCase _updatePromotionUseCase;
        private readonly DeletePromotionUseCase _deletePromotionUseCase;
        private readonly AddGameToPromotionUseCase _addGameToPromotionUseCase;

        public PromotionsController(
            GetPromotionsUseCase getPromotionsUseCase,
            GetPromotionByIdUseCase getPromotionByIdUseCase,
            CreatePromotionUseCase createPromotionUseCase,
            UpdatePromotionUseCase updatePromotionUseCase,
            DeletePromotionUseCase deletePromotionUseCase,
            AddGameToPromotionUseCase addGameToPromotionUseCase)
        {
            _getPromotionsUseCase = getPromotionsUseCase;
            _getPromotionByIdUseCase = getPromotionByIdUseCase;
            _createPromotionUseCase = createPromotionUseCase;
            _updatePromotionUseCase = updatePromotionUseCase;
            _deletePromotionUseCase = deletePromotionUseCase;
            _addGameToPromotionUseCase = addGameToPromotionUseCase;
        }

        /// <summary>
        /// List all promotions.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<PromotionResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IReadOnlyList<PromotionResponse>>> GetAll()
        {
            var promotions = await _getPromotionsUseCase.ExecuteAsync();
            var response = promotions.Select(MapToResponse).ToList();
            return Ok(response);
        }

        /// <summary>
        /// Get a promotion by id.
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(PromotionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<PromotionResponse>> GetById(Guid id)
        {
            var promotion = await _getPromotionByIdUseCase.ExecuteAsync(id);
            if (promotion is null)
            {
                return NotFound();
            }

            return Ok(MapToResponse(promotion));
        }

        /// <summary>
        /// Create a new promotion.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(PromotionResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<PromotionResponse>> Create([FromBody] CreatePromotionRequest request)
        {
            var result = await _createPromotionUseCase.ExecuteAsync(new CreatePromotionInput
            {
                Name = request.Name,
                Description = request.Description,
                DiscountPercentage = request.DiscountPercentage,
                StartsAt = request.StartsAt,
                EndsAt = request.EndsAt
            });

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var response = MapToResponse(result.Data!);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        /// <summary>
        /// Update a promotion by id.
        /// </summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(PromotionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<PromotionResponse>> Update(Guid id, [FromBody] UpdatePromotionRequest request)
        {
            var result = await _updatePromotionUseCase.ExecuteAsync(id, new UpdatePromotionInput
            {
                Name = request.Name,
                Description = request.Description,
                DiscountPercentage = request.DiscountPercentage,
                StartsAt = request.StartsAt,
                EndsAt = request.EndsAt,
                IsActive = request.IsActive
            });

            if (!result.Success)
            {
                return result.Error switch
                {
                    PromotionError.NotFound => NotFound(result.Message),
                    PromotionError.Validation => BadRequest(result.Message),
                    _ => BadRequest(result.Message)
                };
            }

            return Ok(MapToResponse(result.Data!));
        }

        /// <summary>
        /// Delete a promotion by id.
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _deletePromotionUseCase.ExecuteAsync(id);
            if (!result.Success)
            {
                return NotFound(result.Message);
            }

            return NoContent();
        }

        /// <summary>
        /// Add a game to a promotion.
        /// </summary>
        [HttpPost("{id:guid}/games")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> AddGame(Guid id, [FromBody] AddPromotionGameRequest request)
        {
            var result = await _addGameToPromotionUseCase.ExecuteAsync(id, request.GameId);
            if (!result.Success)
            {
                return result.Error switch
                {
                    PromotionError.NotFound => NotFound(result.Message),
                    PromotionError.Conflict => Conflict(result.Message),
                    PromotionError.Validation => BadRequest(result.Message),
                    _ => BadRequest(result.Message)
                };
            }

            return NoContent();
        }

        private static PromotionResponse MapToResponse(PromotionDto promotion)
        {
            return new PromotionResponse
            {
                Id = promotion.Id,
                Name = promotion.Name,
                Description = promotion.Description,
                DiscountPercentage = promotion.DiscountPercentage,
                StartsAt = promotion.StartsAt,
                EndsAt = promotion.EndsAt,
                IsActive = promotion.IsActive,
                GameIds = promotion.GameIds
            };
        }
    }
}
