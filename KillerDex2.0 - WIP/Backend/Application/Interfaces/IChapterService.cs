using Application.DTOs;
using Application.DTOs.Requests;

namespace Application.Interfaces;

public interface IChapterService
{
    Task<IEnumerable<ChapterSummaryDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ChapterDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ChapterDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<ChapterDto?> GetByNumberAsync(int number, CancellationToken cancellationToken = default);

    Task<ChapterDto> CreateAsync(CreateChapterRequest request, CancellationToken cancellationToken = default);
    Task<ChapterDto?> UpdateAsync(Guid id, UpdateChapterRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
