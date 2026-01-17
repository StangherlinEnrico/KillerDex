using Application.DTOs;

namespace Application.Interfaces;

public interface IChapterService
{
    Task<IEnumerable<ChapterSummaryDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ChapterDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ChapterDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<ChapterDto?> GetByNumberAsync(int number, CancellationToken cancellationToken = default);
}
