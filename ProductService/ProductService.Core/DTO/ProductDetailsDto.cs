namespace ProductService.Core.DTO
{
    public record ProductDetailsDto
    (
        int Id,
        string Name,
        decimal Price,
        string Category,
        int Quantity
    );
}