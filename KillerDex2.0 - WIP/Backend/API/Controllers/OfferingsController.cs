using Application.DTOs;
using Application.DTOs.Requests;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class OfferingsController : ControllerBase
{
    private readonly IOfferingService _offeringService;

    public OfferingsController(IOfferingService offeringService)
    {
        _offeringService = offeringService;
    }

    /// <summary>
    /// Get all offerings
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<OfferingSummaryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<OfferingSummaryDto>>> GetAll([FromQuery] string? role, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(role))
        {
            var filtered = await _offeringService.GetByRoleAsync(role, cancellationToken);
            return Ok(filtered);
        }

        var offerings = await _offeringService.GetAllAsync(cancellationToken);
        return Ok(offerings);
    }

    /// <summary>
    /// Get offering by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(OfferingDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OfferingDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var offering = await _offeringService.GetByIdAsync(id, cancellationToken);
        if (offering is null) return NotFound();
        return Ok(offering);
    }

    /// <summary>
    /// Get offering by slug
    /// </summary>
    [HttpGet("{slug}")]
    [ProducesResponseType(typeof(OfferingDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OfferingDto>> GetBySlug(string slug, CancellationToken cancellationToken)
    {
        var offering = await _offeringService.GetBySlugAsync(slug, cancellationToken);
        if (offering is null) return NotFound();
        return Ok(offering);
    }

    /// <summary>
    /// Create a new offering (requires API Key)
    /// </summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(OfferingDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<OfferingDto>> Create([FromBody] CreateOfferingRequest request, CancellationToken cancellationToken)
    {
        var offering = await _offeringService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = offering.Id }, offering);
    }

    /// <summary>
    /// Update an existing offering (requires API Key)
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(OfferingDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OfferingDto>> Update(Guid id, [FromBody] UpdateOfferingRequest request, CancellationToken cancellationToken)
    {
        var offering = await _offeringService.UpdateAsync(id, request, cancellationToken);
        if (offering is null) return NotFound();
        return Ok(offering);
    }

    /// <summary>
    /// Delete an offering (requires API Key)
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _offeringService.DeleteAsync(id, cancellationToken);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
