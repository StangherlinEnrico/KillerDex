using Application.DTOs;
using Application.DTOs.Requests;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/realms")]
[Produces("application/json")]
public class RealmsController : ControllerBase
{
    private readonly IRealmService _realmService;

    public RealmsController(IRealmService realmService)
    {
        _realmService = realmService;
    }

    /// <summary>
    /// Get all realms
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RealmSummaryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<RealmSummaryDto>>> GetAll(CancellationToken cancellationToken)
    {
        var realms = await _realmService.GetAllAsync(cancellationToken);
        return Ok(realms);
    }

    /// <summary>
    /// Get realm by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(RealmDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RealmDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var realm = await _realmService.GetByIdAsync(id, cancellationToken);
        if (realm is null) return NotFound();
        return Ok(realm);
    }

    /// <summary>
    /// Get realm by slug
    /// </summary>
    [HttpGet("{slug}")]
    [ProducesResponseType(typeof(RealmDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RealmDto>> GetBySlug(string slug, CancellationToken cancellationToken)
    {
        var realm = await _realmService.GetBySlugAsync(slug, cancellationToken);
        if (realm is null) return NotFound();
        return Ok(realm);
    }

    /// <summary>
    /// Create a new realm (requires API Key)
    /// </summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(RealmDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<RealmDto>> Create([FromBody] CreateRealmRequest request, CancellationToken cancellationToken)
    {
        var realm = await _realmService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = realm.Id }, realm);
    }

    /// <summary>
    /// Update an existing realm (requires API Key)
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(RealmDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RealmDto>> Update(Guid id, [FromBody] UpdateRealmRequest request, CancellationToken cancellationToken)
    {
        var realm = await _realmService.UpdateAsync(id, request, cancellationToken);
        if (realm is null) return NotFound();
        return Ok(realm);
    }

    /// <summary>
    /// Delete a realm (requires API Key)
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _realmService.DeleteAsync(id, cancellationToken);
        if (!deleted) return NotFound();
        return NoContent();
    }
}

[ApiController]
[Route("api/maps")]
[Produces("application/json")]
public class MapsController : ControllerBase
{
    private readonly IMapService _mapService;

    public MapsController(IMapService mapService)
    {
        _mapService = mapService;
    }

    /// <summary>
    /// Get all maps
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<MapSummaryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MapSummaryDto>>> GetAll([FromQuery] Guid? realmId, CancellationToken cancellationToken)
    {
        if (realmId.HasValue)
        {
            var filtered = await _mapService.GetByRealmIdAsync(realmId.Value, cancellationToken);
            return Ok(filtered);
        }

        var maps = await _mapService.GetAllAsync(cancellationToken);
        return Ok(maps);
    }

    /// <summary>
    /// Get map by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(MapDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MapDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var map = await _mapService.GetByIdAsync(id, cancellationToken);
        if (map is null) return NotFound();
        return Ok(map);
    }

    /// <summary>
    /// Get map by slug
    /// </summary>
    [HttpGet("{slug}")]
    [ProducesResponseType(typeof(MapDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MapDto>> GetBySlug(string slug, CancellationToken cancellationToken)
    {
        var map = await _mapService.GetBySlugAsync(slug, cancellationToken);
        if (map is null) return NotFound();
        return Ok(map);
    }

    /// <summary>
    /// Create a new map (requires API Key)
    /// </summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(MapDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<MapDto>> Create([FromBody] CreateMapRequest request, CancellationToken cancellationToken)
    {
        var map = await _mapService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = map.Id }, map);
    }

    /// <summary>
    /// Update an existing map (requires API Key)
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(MapDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MapDto>> Update(Guid id, [FromBody] UpdateMapRequest request, CancellationToken cancellationToken)
    {
        var map = await _mapService.UpdateAsync(id, request, cancellationToken);
        if (map is null) return NotFound();
        return Ok(map);
    }

    /// <summary>
    /// Delete a map (requires API Key)
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _mapService.DeleteAsync(id, cancellationToken);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
