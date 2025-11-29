using ProductService.Infrastructure.Entities;

namespace ProductService.Infrastructure.Repositories
{
    public interface IForbiddenWordRepository
    {
        Task<IEnumerable<ForbiddenWord>> GetAllAsync();
    }
}
