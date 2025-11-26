namespace ProductService.Core.DTO
{
    public record ProductCreateDto
    (
        string Name,
        decimal Price,
        string Category,
        int Quantity
    );
}