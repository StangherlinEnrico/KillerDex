using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ChaptersController : ControllerBase
{
    private readonly IChapterService _chapterService;

    public ChaptersController(IChapterService chapterService)
    {
        _chapterService = chapterService;
    }

    /// <summary>
    /// Get all chapters
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ChapterSummaryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ChapterSummaryDto>>> GetAll(CancellationToken cancellationToken)
    {
        var chapters = await _chapterService.GetAllAsync(cancellationToken);
        return Ok(chapters);
    }

    /// <summary>
    /// Get chapter by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ChapterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ChapterDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var chapter = await _chapterService.GetByIdAsync(id, cancellationToken);
        if (chapter is null) return NotFound();
        return Ok(chapter);
    }

    /// <summary>
    /// Get chapter by slug
    /// </summary>
    [HttpGet("{slug}")]
    [ProducesResponseType(typeof(ChapterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ChapterDto>> GetBySlug(string slug, CancellationToken cancellationToken)
    {
        var chapter = await _chapterService.GetBySlugAsync(slug, cancellationToken);
        if (chapter is null) return NotFound();
        return Ok(chapter);
    }

    /// <summary>
    /// Get chapter by number
    /// </summary>
    [HttpGet("number/{number:int}")]
    [ProducesResponseType(typeof(ChapterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ChapterDto>> GetByNumber(int number, CancellationToken cancellationToken)
    {
        var chapter = await _chapterService.GetByNumberAsync(number, cancellationToken);
        if (chapter is null) return NotFound();
        return Ok(chapter);
    }
}
