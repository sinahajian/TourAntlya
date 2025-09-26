
using Models.Entities;

namespace Models.Interface
{

    public interface ITourRepository
    {
        List<Tour> GetAll();
        Task<TourDto?> GetByIdAsync(int id, string lang, CancellationToken ct = default);
        Task<List<TourDto>> ListAsync(string lang, CancellationToken ct = default);

    }
}