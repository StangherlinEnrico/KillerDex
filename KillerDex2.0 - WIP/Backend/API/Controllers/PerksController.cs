using Application.DTOs;
using Application.DTOs.Requests;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PerksController : ControllerBase
{
    private readonly IPerkService _perkService;

    public PerksController(IPerkService perkService)
    {
        _perkService = perkService;
    }

    /// <summary>
    /// Get all perks
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PerkSummaryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PerkSummaryDto>>> GetAll([FromQuery] string? role, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(role))
        {
            var filtered = await _perkService.GetByRoleAsync(role, cancellationToken);
            return Ok(filtered);
        }

        var perks = await _perkService.GetAllAsync(cancellationToken);
        return Ok(perks);
    }

    /// <summary>
    /// Get perk by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(PerkDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PerkDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var perk = await _perkService.GetByIdAsync(id, cancellationToken);
        if (perk is null) return NotFound();
        return Ok(perk);
    }

    /// <summary>
    /// Get perk by slug
    /// </summary>
    [HttpGet("{slug}")]
    [ProducesResponseType(typeof(PerkDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PerkDto>> GetBySlug(string slug, CancellationToken cancellationToken)
    {
        var perk = await _perkService.GetBySlugAsync(slug, cancellationToken);
        if (perk is null) return NotFound();
        return Ok(perk);
    }

    /// <summary>
    /// Create a new perk (requires API Key)
    /// </summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(PerkDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<PerkDto>> Create([FromBody] CreatePerkRequest request, CancellationToken cancellationToken)
    {
        var perk = await _perkService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = perk.Id }, perk);
    }

    /// <summary>
    /// Update an existing perk (requires API Key)
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(PerkDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PerkDto>> Update(Guid id, [FromBody] UpdatePerkRequest request, CancellationToken cancellationToken)
    {
        var perk = await _perkService.UpdateAsync(id, request, cancellationToken);
        if (perk is null) return NotFound();
        return Ok(perk);
    }

    /// <summary>
    /// Delete a perk (requires API Key)
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _perkService.DeleteAsync(id, cancellationToken);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
