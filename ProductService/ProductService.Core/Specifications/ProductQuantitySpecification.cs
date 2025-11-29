using ProductService.Infrastructure.Entities;

namespace ProductService.Core.Specifications
{
    internal class ProductQuantitySpecification : ISpecification<Product>
    {
        public bool IsSatisfiedBy(Product validatedObject)
        {
            return validatedObject.Quantity >= 0;
        }

        public string GetErrorMessage()
        {
            return "Product quantity must be greater than or equal to 0";
        }
    }
}
