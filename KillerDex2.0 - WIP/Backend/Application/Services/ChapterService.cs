using Application.DTOs;
using Application.DTOs.Requests;
using Application.Interfaces;
using Application.Mappings;
using Domain.Entities;
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

    public async Task<ChapterDto> CreateAsync(CreateChapterRequest request, CancellationToken cancellationToken = default)
    {
        var chapter = new Chapter(
            name: request.Name,
            number: request.Number,
            releaseDate: request.ReleaseDate,
            gameVersion: request.GameVersion
        );

        if (request.ImageUrl is not null)
            chapter.SetImageUrl(request.ImageUrl);

        _context.Chapters.Add(chapter);
        await _context.SaveChangesAsync(cancellationToken);

        return chapter.ToDto([], []);
    }

    public async Task<ChapterDto?> UpdateAsync(Guid id, UpdateChapterRequest request, CancellationToken cancellationToken = default)
    {
        var chapter = await _context.Chapters.FindAsync([id], cancellationToken);

        if (chapter is null)
            return null;

        chapter.Update(
            name: request.Name,
            number: request.Number,
            releaseDate: request.ReleaseDate
        );

        if (request.ImageUrl is not null)
            chapter.SetImageUrl(request.ImageUrl);

        if (request.GameVersion is not null)
            chapter.SetGameVersion(request.GameVersion);

        await _context.SaveChangesAsync(cancellationToken);

        return await GetChapterWithRelationsAsync(chapter, cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var chapter = await _context.Chapters.FindAsync([id], cancellationToken);

        if (chapter is null)
            return false;

        _context.Chapters.Remove(chapter);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
