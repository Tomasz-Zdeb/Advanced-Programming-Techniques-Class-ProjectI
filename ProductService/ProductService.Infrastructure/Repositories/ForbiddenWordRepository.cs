using Microsoft.EntityFrameworkCore;
using ProductService.Infrastructure.Entities;

namespace ProductService.Infrastructure.Repositories
{
    internal class ForbiddenWordRepository : IForbiddenWordRepository
    {
        private readonly ApplicationDbContext _context;

        public ForbiddenWordRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ForbiddenWord>> GetAllAsync()
        {
            return await _context.ForbiddenWords.ToListAsync();
        }
    }
}
