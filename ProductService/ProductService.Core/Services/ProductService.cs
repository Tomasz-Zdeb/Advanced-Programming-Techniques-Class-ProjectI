using ProductService.Core.DTO;
using ProductService.Infrastructure.Entities;
using ProductService.Infrastructure.Repositories;

namespace ProductService.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductListDto>> GetAllAsync()
        {
            var products = await _repository.GetAllAsync();
            return products.Select(p => new ProductListDto(
                p.Id,
                p.Name,
                p.Price,
                p.Category.ToString()
            ));
        }

        public async Task<ProductDetailsDto?> GetByIdAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null) return null;

            return new ProductDetailsDto(
                product.Id,
                product.Name,
                product.Price,
                product.Category.ToString(),
                product.Quantity
            );
        }

        public async Task<ProductDetailsDto> CreateAsync(ProductCreateDto dto)
        {
            // Prawodopodobnie w tym miejscu będzie miała walidacja po implemetacji wzorca specyfikacji

            var product = new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                Category = Enum.Parse<ProductCategory>(dto.Category),
                Quantity = dto.Quantity
            };

            var created = await _repository.AddAsync(product);

            return new ProductDetailsDto(
                created.Id,
                created.Name,
                created.Price,
                created.Category.ToString(),
                created.Quantity
            );
        }

        public async Task<ProductDetailsDto> UpdateAsync(int id, ProductUpdateDto dto)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
                throw new KeyNotFoundException($"Product with id {id} not found");

            // Prawodopodobnie w tym miejscu będzie miała walidacja po implemetacji wzorca specyfikacji

            product.Name = dto.Name;
            product.Price = dto.Price;
            product.Category = Enum.Parse<ProductCategory>(dto.Category);
            product.Quantity = dto.Quantity;

            var updated = await _repository.UpdateAsync(product);

            return new ProductDetailsDto(
                updated.Id,
                updated.Name,
                updated.Price,
                updated.Category.ToString(),
                updated.Quantity
            );
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
