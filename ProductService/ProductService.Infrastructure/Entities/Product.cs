namespace ProductService.Infrastructure.Entities
{
// Oczywiście klasa produktu pod żadnym pozorem nie będzie mieć tak banalnej formy. Aktualnie wygląda w ten uproszczony sposób na potrzeby konfiguracji
// połączenia z bazą danych za pomocą Entity Framework
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
