using ProductService.Infrastructure.Entities;

namespace ProductService.Core.Specifications
{
    internal class ProductNameAlphanumericSpecification : ISpecification<Product>
    {
        public bool IsSatisfiedBy(Product validatedObject)
        {
            return validatedObject.Name.All(char.IsLetterOrDigit);
        }

        public string GetErrorMessage()
        {
            return "Product name must contain only letters and digits";
        }
    }
}
