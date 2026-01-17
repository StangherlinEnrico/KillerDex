using Application.DTOs;
using Application.Interfaces;
using Application.Mappings;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class ChapterService : IChapterService
{
    private readonly DbdContext _context;

    public ChapterService(DbdContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ChapterSummaryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var chapters = await _context.Chapters
            .OrderBy(c => c.Number)
            .ToListAsync(cancellationToken);

        return chapters.Select(c => c.ToSummaryDto());
    }

    public async Task<ChapterDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var chapter = await _context.Chapters
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (chapter is null) return null;

        return await GetChapterWithRelationsAsync(chapter, cancellationToken);
    }

    public async Task<ChapterDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var chapter = await _context.Chapters
            .FirstOrDefaultAsync(c => c.Slug == slug.ToLowerInvariant(), cancellationToken);

        if (chapter is null) return null;

        return await GetChapterWithRelationsAsync(chapter, cancellationToken);
    }

    public async Task<ChapterDto?> GetByNumberAsync(int number, CancellationToken cancellationToken = default)
    {
        var chapter = await _context.Chapters
            .FirstOrDefaultAsync(c => c.Number == number, cancellationToken);

        if (chapter is null) return null;

        return await GetChapterWithRelationsAsync(chapter, cancellationToken);
    }

    private async Task<ChapterDto> GetChapterWithRelationsAsync(Domain.Entities.Chapter chapter, CancellationToken cancellationToken)
    {
        var killers = await _context.Killers
            .Where(k => k.ChapterId == chapter.Id)
            .ToListAsync(cancellationToken);

        var survivors = await _context.Survivors
            .Where(s => s.ChapterId == chapter.Id)
            .ToListAsync(cancellationToken);

        return chapter.ToDto(killers, survivors);
    }
}
