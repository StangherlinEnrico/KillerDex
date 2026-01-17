using Application.DTOs;
using Application.DTOs.Requests;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

    /// <summary>
    /// Create a new chapter (requires API Key)
    /// </summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(ChapterDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ChapterDto>> Create([FromBody] CreateChapterRequest request, CancellationToken cancellationToken)
    {
        var chapter = await _chapterService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = chapter.Id }, chapter);
    }

    /// <summary>
    /// Update an existing chapter (requires API Key)
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(ChapterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ChapterDto>> Update(Guid id, [FromBody] UpdateChapterRequest request, CancellationToken cancellationToken)
    {
        var chapter = await _chapterService.UpdateAsync(id, request, cancellationToken);
        if (chapter is null) return NotFound();
        return Ok(chapter);
    }

    /// <summary>
    /// Delete a chapter (requires API Key)
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _chapterService.DeleteAsync(id, cancellationToken);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
