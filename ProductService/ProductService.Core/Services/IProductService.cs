using ProductService.Core.DTO;

namespace ProductService.Core.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductListDto>> GetAllAsync();
        Task<ProductDetailsDto?> GetByIdAsync(int id);
        Task<ProductDetailsDto> CreateAsync(ProductCreateDto dto);
        Task<ProductDetailsDto> UpdateAsync(int id, ProductUpdateDto dto);
        Task DeleteAsync(int id);
    }
}
