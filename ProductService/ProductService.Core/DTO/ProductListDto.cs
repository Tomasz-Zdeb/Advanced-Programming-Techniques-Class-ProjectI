using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Core.DTO
{
    public record ProductListDto(
        int Id,
        string Name,
        decimal Price,
        string Category
    );
}
