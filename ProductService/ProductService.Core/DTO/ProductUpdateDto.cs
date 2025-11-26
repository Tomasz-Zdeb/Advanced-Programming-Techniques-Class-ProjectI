namespace ProductService.Core.DTO
{
    public record ProductUpdateDto
    (
        string Name,
        decimal Price,
        string Category,
        int Quantity
    );
}
