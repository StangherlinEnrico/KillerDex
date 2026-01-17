using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ItemsController : ControllerBase
{
    private readonly IItemService _itemService;

    public ItemsController(IItemService itemService)
    {
        _itemService = itemService;
    }

    /// <summary>
    /// Get all items
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ItemSummaryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ItemSummaryDto>>> GetAll([FromQuery] string? type, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(type))
        {
            var filtered = await _itemService.GetByTypeAsync(type, cancellationToken);
            return Ok(filtered);
        }

        var items = await _itemService.GetAllAsync(cancellationToken);
        return Ok(items);
    }

    /// <summary>
    /// Get item by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ItemDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var item = await _itemService.GetByIdAsync(id, cancellationToken);
        if (item is null) return NotFound();
        return Ok(item);
    }

    /// <summary>
    /// Get item by slug
    /// </summary>
    [HttpGet("{slug}")]
    [ProducesResponseType(typeof(ItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ItemDto>> GetBySlug(string slug, CancellationToken cancellationToken)
    {
        var item = await _itemService.GetBySlugAsync(slug, cancellationToken);
        if (item is null) return NotFound();
        return Ok(item);
    }

    /// <summary>
    /// Get addons for item type
    /// </summary>
    [HttpGet("type/{itemType}/addons")]
    [ProducesResponseType(typeof(IEnumerable<AddonSummaryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AddonSummaryDto>>> GetAddons(string itemType, CancellationToken cancellationToken)
    {
        var addons = await _itemService.GetAddonsAsync(itemType, cancellationToken);
        return Ok(addons);
    }
}
