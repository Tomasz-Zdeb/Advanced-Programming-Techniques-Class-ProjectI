using ProductService.Infrastructure.Entities;

namespace ProductService.Core.Specifications
{
    internal class ProductNameLengthSpecification : ISpecification<Product>
    {
        public bool IsSatisfiedBy(Product validatedObject)
        {
            return validatedObject.Name.Length >= 3 && validatedObject.Name.Length <= 20;
        }

        public string GetErrorMessage()
        {
            return "Product name must be between 3 and 20 characters";
        }
    }
}
