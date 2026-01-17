using Application.DTOs;
using Application.DTOs.Requests;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class KillersController : ControllerBase
{
    private readonly IKillerService _killerService;

    public KillersController(IKillerService killerService)
    {
        _killerService = killerService;
    }

    /// <summary>
    /// Get all killers
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<KillerSummaryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<KillerSummaryDto>>> GetAll(CancellationToken cancellationToken)
    {
        var killers = await _killerService.GetAllAsync(cancellationToken);
        return Ok(killers);
    }

    /// <summary>
    /// Get killer by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(KillerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<KillerDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var killer = await _killerService.GetByIdAsync(id, cancellationToken);
        if (killer is null) return NotFound();
        return Ok(killer);
    }

    /// <summary>
    /// Get killer by slug
    /// </summary>
    [HttpGet("{slug}")]
    [ProducesResponseType(typeof(KillerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<KillerDto>> GetBySlug(string slug, CancellationToken cancellationToken)
    {
        var killer = await _killerService.GetBySlugAsync(slug, cancellationToken);
        if (killer is null) return NotFound();
        return Ok(killer);
    }

    /// <summary>
    /// Get killer's perks
    /// </summary>
    [HttpGet("{id:guid}/perks")]
    [ProducesResponseType(typeof(IEnumerable<PerkSummaryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PerkSummaryDto>>> GetPerks(Guid id, CancellationToken cancellationToken)
    {
        var perks = await _killerService.GetPerksAsync(id, cancellationToken);
        return Ok(perks);
    }

    /// <summary>
    /// Get killer's addons
    /// </summary>
    [HttpGet("{id:guid}/addons")]
    [ProducesResponseType(typeof(IEnumerable<AddonSummaryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AddonSummaryDto>>> GetAddons(Guid id, CancellationToken cancellationToken)
    {
        var addons = await _killerService.GetAddonsAsync(id, cancellationToken);
        return Ok(addons);
    }

    /// <summary>
    /// Create a new killer (requires API Key)
    /// </summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(KillerDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<KillerDto>> Create([FromBody] CreateKillerRequest request, CancellationToken cancellationToken)
    {
        var killer = await _killerService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = killer.Id }, killer);
    }

    /// <summary>
    /// Update an existing killer (requires API Key)
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(KillerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<KillerDto>> Update(Guid id, [FromBody] UpdateKillerRequest request, CancellationToken cancellationToken)
    {
        var killer = await _killerService.UpdateAsync(id, request, cancellationToken);
        if (killer is null) return NotFound();
        return Ok(killer);
    }

    /// <summary>
    /// Delete a killer (requires API Key)
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _killerService.DeleteAsync(id, cancellationToken);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
