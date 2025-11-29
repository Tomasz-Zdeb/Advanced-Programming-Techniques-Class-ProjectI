using ProductService.Infrastructure.Entities;
using ProductService.Infrastructure.Repositories;

namespace ProductService.Core.Specifications
{
    internal class UniqueProductNameSpecification : ISpecification<Product>
    {
        private readonly IProductRepository _repository;
        private readonly int? _currentProductId;

        public UniqueProductNameSpecification(IProductRepository repository, int? currentProductId = null)
        {
            _repository = repository;
            _currentProductId = currentProductId;
        }

        public bool IsSatisfiedBy(Product validatedObject)
        {
            var existing = _repository.GetByNameAsync(validatedObject.Name).Result;

            if (existing == null)
                return true;

            if (_currentProductId.HasValue && existing.Id == _currentProductId.Value)
                return true;

            return false;
        }

        public string GetErrorMessage()
        {
            return "Product with this name already exists";
        }
    }
}
