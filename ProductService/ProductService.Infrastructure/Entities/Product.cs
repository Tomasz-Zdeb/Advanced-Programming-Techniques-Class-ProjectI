namespace ProductService.Infrastructure.Entities
{
    // Chcąc dalej rozwijać model poza minimalne wymagania wynikające z polecenia zadania
    // możnaby np. zastanowić się nad utworzeniem pola określającego walutę.
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public ProductCategory Category { get; set; }
        public int Quantity { get; set; }
    }
}
