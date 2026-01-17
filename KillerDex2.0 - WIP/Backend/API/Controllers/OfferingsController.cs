using Application.DTOs;
using Application.Interfaces;
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
}
