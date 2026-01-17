using Application.DTOs;
using Application.DTOs.Requests;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/killer-addons")]
[Produces("application/json")]
public class KillerAddonsController : ControllerBase
{
    private readonly IKillerAddonService _addonService;

    public KillerAddonsController(IKillerAddonService addonService)
    {
        _addonService = addonService;
    }

    /// <summary>
    /// Get all killer addons
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AddonSummaryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AddonSummaryDto>>> GetAll([FromQuery] Guid? killerId, CancellationToken cancellationToken)
    {
        if (killerId.HasValue)
        {
            var filtered = await _addonService.GetByKillerIdAsync(killerId.Value, cancellationToken);
            return Ok(filtered);
        }

        var addons = await _addonService.GetAllAsync(cancellationToken);
        return Ok(addons);
    }

    /// <summary>
    /// Get killer addon by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(KillerAddonDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<KillerAddonDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var addon = await _addonService.GetByIdAsync(id, cancellationToken);
        if (addon is null) return NotFound();
        return Ok(addon);
    }

    /// <summary>
    /// Get killer addon by slug
    /// </summary>
    [HttpGet("{slug}")]
    [ProducesResponseType(typeof(KillerAddonDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<KillerAddonDto>> GetBySlug(string slug, CancellationToken cancellationToken)
    {
        var addon = await _addonService.GetBySlugAsync(slug, cancellationToken);
        if (addon is null) return NotFound();
        return Ok(addon);
    }

    /// <summary>
    /// Create a new killer addon (requires API Key)
    /// </summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(KillerAddonDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<KillerAddonDto>> Create([FromBody] CreateKillerAddonRequest request, CancellationToken cancellationToken)
    {
        var addon = await _addonService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = addon.Id }, addon);
    }

    /// <summary>
    /// Update an existing killer addon (requires API Key)
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(KillerAddonDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<KillerAddonDto>> Update(Guid id, [FromBody] UpdateKillerAddonRequest request, CancellationToken cancellationToken)
    {
        var addon = await _addonService.UpdateAsync(id, request, cancellationToken);
        if (addon is null) return NotFound();
        return Ok(addon);
    }

    /// <summary>
    /// Delete a killer addon (requires API Key)
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _addonService.DeleteAsync(id, cancellationToken);
        if (!deleted) return NotFound();
        return NoContent();
    }
}

[ApiController]
[Route("api/survivor-addons")]
[Produces("application/json")]
public class SurvivorAddonsController : ControllerBase
{
    private readonly ISurvivorAddonService _addonService;

    public SurvivorAddonsController(ISurvivorAddonService addonService)
    {
        _addonService = addonService;
    }

    /// <summary>
    /// Get all survivor addons
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AddonSummaryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AddonSummaryDto>>> GetAll([FromQuery] string? itemType, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(itemType))
        {
            var filtered = await _addonService.GetByItemTypeAsync(itemType, cancellationToken);
            return Ok(filtered);
        }

        var addons = await _addonService.GetAllAsync(cancellationToken);
        return Ok(addons);
    }

    /// <summary>
    /// Get survivor addon by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(SurvivorAddonDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SurvivorAddonDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var addon = await _addonService.GetByIdAsync(id, cancellationToken);
        if (addon is null) return NotFound();
        return Ok(addon);
    }

    /// <summary>
    /// Get survivor addon by slug
    /// </summary>
    [HttpGet("{slug}")]
    [ProducesResponseType(typeof(SurvivorAddonDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SurvivorAddonDto>> GetBySlug(string slug, CancellationToken cancellationToken)
    {
        var addon = await _addonService.GetBySlugAsync(slug, cancellationToken);
        if (addon is null) return NotFound();
        return Ok(addon);
    }

    /// <summary>
    /// Create a new survivor addon (requires API Key)
    /// </summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(SurvivorAddonDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<SurvivorAddonDto>> Create([FromBody] CreateSurvivorAddonRequest request, CancellationToken cancellationToken)
    {
        var addon = await _addonService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = addon.Id }, addon);
    }

    /// <summary>
    /// Update an existing survivor addon (requires API Key)
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(SurvivorAddonDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SurvivorAddonDto>> Update(Guid id, [FromBody] UpdateSurvivorAddonRequest request, CancellationToken cancellationToken)
    {
        var addon = await _addonService.UpdateAsync(id, request, cancellationToken);
        if (addon is null) return NotFound();
        return Ok(addon);
    }

    /// <summary>
    /// Delete a survivor addon (requires API Key)
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _addonService.DeleteAsync(id, cancellationToken);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
