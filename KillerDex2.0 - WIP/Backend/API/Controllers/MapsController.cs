using Application.DTOs;
using Application.Interfaces;
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
}
