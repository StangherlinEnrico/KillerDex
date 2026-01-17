using Application.DTOs;
using Application.DTOs.Requests;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class SurvivorsController : ControllerBase
{
    private readonly ISurvivorService _survivorService;

    public SurvivorsController(ISurvivorService survivorService)
    {
        _survivorService = survivorService;
    }

    /// <summary>
    /// Get all survivors
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SurvivorSummaryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SurvivorSummaryDto>>> GetAll(CancellationToken cancellationToken)
    {
        var survivors = await _survivorService.GetAllAsync(cancellationToken);
        return Ok(survivors);
    }

    /// <summary>
    /// Get survivor by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(SurvivorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SurvivorDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var survivor = await _survivorService.GetByIdAsync(id, cancellationToken);
        if (survivor is null) return NotFound();
        return Ok(survivor);
    }

    /// <summary>
    /// Get survivor by slug
    /// </summary>
    [HttpGet("{slug}")]
    [ProducesResponseType(typeof(SurvivorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SurvivorDto>> GetBySlug(string slug, CancellationToken cancellationToken)
    {
        var survivor = await _survivorService.GetBySlugAsync(slug, cancellationToken);
        if (survivor is null) return NotFound();
        return Ok(survivor);
    }

    /// <summary>
    /// Get survivor's perks
    /// </summary>
    [HttpGet("{id:guid}/perks")]
    [ProducesResponseType(typeof(IEnumerable<PerkSummaryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PerkSummaryDto>>> GetPerks(Guid id, CancellationToken cancellationToken)
    {
        var perks = await _survivorService.GetPerksAsync(id, cancellationToken);
        return Ok(perks);
    }

    /// <summary>
    /// Create a new survivor (requires API Key)
    /// </summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(SurvivorDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<SurvivorDto>> Create([FromBody] CreateSurvivorRequest request, CancellationToken cancellationToken)
    {
        var survivor = await _survivorService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = survivor.Id }, survivor);
    }

    /// <summary>
    /// Update an existing survivor (requires API Key)
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(SurvivorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SurvivorDto>> Update(Guid id, [FromBody] UpdateSurvivorRequest request, CancellationToken cancellationToken)
    {
        var survivor = await _survivorService.UpdateAsync(id, request, cancellationToken);
        if (survivor is null) return NotFound();
        return Ok(survivor);
    }

    /// <summary>
    /// Delete a survivor (requires API Key)
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _survivorService.DeleteAsync(id, cancellationToken);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
