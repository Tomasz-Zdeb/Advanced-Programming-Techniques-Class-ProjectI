using ProductService.Core.DTO;
using ProductService.Core.Exceptions;
using ProductService.Core.Specifications;
using ProductService.Infrastructure.Entities;
using ProductService.Infrastructure.Repositories;
using System.ComponentModel.DataAnnotations;

namespace ProductService.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IForbiddenWordRepository _forbiddenWordRepository;

        public ProductService(IProductRepository repository,
            IForbiddenWordRepository forbiddenWordRepository)
        {
            _repository = repository;
            _forbiddenWordRepository = forbiddenWordRepository;
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

            var product = new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                Category = Enum.Parse<ProductCategory>(dto.Category),
                Quantity = dto.Quantity
            };

            PerformDomainValidation(product);

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

            product.Name = dto.Name;
            product.Price = dto.Price;
            product.Category = Enum.Parse<ProductCategory>(dto.Category);
            product.Quantity = dto.Quantity;

            PerformDomainValidation(product, id);

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
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
                throw new NotFoundException($"Product with id {id} not found");

            await _repository.DeleteAsync(id);
        }

        private void PerformDomainValidation(Product product, int? currentProductId = null)
        {
            var uniqueSpec = new UniqueProductNameSpecification(_repository, currentProductId);
            if (!uniqueSpec.IsSatisfiedBy(product))
            {
                throw new ConflictException(uniqueSpec.GetErrorMessage());
            }


            var Specs = new ProductNameLengthSpecification()
                .And(new ProductNameAlphanumericSpecification())
                .And(new ProductPriceRangeSpecification())
                .And(new ProductQuantitySpecification())
                .And(new ForbiddenWordsSpecification(_forbiddenWordRepository));

            if (!Specs.IsSatisfiedBy(product))
            {
                throw new ValidationException(Specs.GetErrorMessage());
            }

        }
    }
}
