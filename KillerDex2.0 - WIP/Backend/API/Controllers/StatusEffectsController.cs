using Application.DTOs;
using Application.DTOs.Requests;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/status-effects")]
[Produces("application/json")]
public class StatusEffectsController : ControllerBase
{
    private readonly IStatusEffectService _statusEffectService;

    public StatusEffectsController(IStatusEffectService statusEffectService)
    {
        _statusEffectService = statusEffectService;
    }

    /// <summary>
    /// Get all status effects
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<StatusEffectSummaryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<StatusEffectSummaryDto>>> GetAll([FromQuery] string? type, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(type))
        {
            var filtered = await _statusEffectService.GetByTypeAsync(type, cancellationToken);
            return Ok(filtered);
        }

        var effects = await _statusEffectService.GetAllAsync(cancellationToken);
        return Ok(effects);
    }

    /// <summary>
    /// Get status effect by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(StatusEffectDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StatusEffectDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var effect = await _statusEffectService.GetByIdAsync(id, cancellationToken);
        if (effect is null) return NotFound();
        return Ok(effect);
    }

    /// <summary>
    /// Get status effect by slug
    /// </summary>
    [HttpGet("{slug}")]
    [ProducesResponseType(typeof(StatusEffectDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StatusEffectDto>> GetBySlug(string slug, CancellationToken cancellationToken)
    {
        var effect = await _statusEffectService.GetBySlugAsync(slug, cancellationToken);
        if (effect is null) return NotFound();
        return Ok(effect);
    }

    /// <summary>
    /// Create a new status effect (requires API Key)
    /// </summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(StatusEffectDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<StatusEffectDto>> Create([FromBody] CreateStatusEffectRequest request, CancellationToken cancellationToken)
    {
        var effect = await _statusEffectService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = effect.Id }, effect);
    }

    /// <summary>
    /// Update an existing status effect (requires API Key)
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(StatusEffectDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StatusEffectDto>> Update(Guid id, [FromBody] UpdateStatusEffectRequest request, CancellationToken cancellationToken)
    {
        var effect = await _statusEffectService.UpdateAsync(id, request, cancellationToken);
        if (effect is null) return NotFound();
        return Ok(effect);
    }

    /// <summary>
    /// Delete a status effect (requires API Key)
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _statusEffectService.DeleteAsync(id, cancellationToken);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
