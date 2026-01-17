using Application.DTOs;
using Application.Interfaces;
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
}
