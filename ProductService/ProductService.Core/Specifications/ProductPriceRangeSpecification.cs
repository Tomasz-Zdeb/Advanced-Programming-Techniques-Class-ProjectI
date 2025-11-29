using ProductService.Infrastructure.Entities;

namespace ProductService.Core.Specifications
{
    // Gdyby chcieć rozbudować rozwiązanie, możnaby pobierać zakresy z bazy danych, wtedy byłaby większa elastyczność
    // Dodatkowo możnaby zwracać w komunikacie błędu, jaki jest prawidłowy zakres wartości dla danej
    // Tak jak jest to zaimplementowane dla mechanizmu walidowania zabronionych słów
    internal class ProductPriceRangeSpecification : ISpecification<Product>
    {
        private static readonly Dictionary<ProductCategory, (decimal Min, decimal Max)> PriceRanges = new()
    {
        { ProductCategory.Electronics, (50m, 50000m) },
        { ProductCategory.Books, (5m, 500m) },
        { ProductCategory.Clothing, (10m, 5000m) }
    };

        public bool IsSatisfiedBy(Product validatedObject)
        {
            if (!PriceRanges.TryGetValue(validatedObject.Category, out var range))
                return false;

            return validatedObject.Price >= range.Min && validatedObject.Price <= range.Max;
        }

        public string GetErrorMessage()
        {
            return "Product price is outside the allowed range for its category";
        }
    }
}
