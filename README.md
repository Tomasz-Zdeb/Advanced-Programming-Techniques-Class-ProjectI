# Zadanie 1 - Tomasz Zdeb

Zgodnie z poleceniem zadania, będę starał implementować aplikację zgodnie z architekturą [N-Layer](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures).

## Inicjalizacja Projektu

Niniejsze komendy skutkują utworzeniem rozwiązania, projektów, dodaniem projektów do rozwiązania i konfiguracji relacji między projektami.

```powershell
dotnet new sln -n ProductService
dotnet new webapi -n ProductService.API
dotnet new classlib -n ProductService.Core
dotnet new classlib -n ProductService.Infrastructure
dotnet sln add .\ProductService.API\ProductService.API.csproj
dotnet sln add .\ProductService.Core\ProductService.Core.csproj
dotnet sln add .\ProductService.Infrastructure\ProductService.Infrastructure.csproj
# Z katalogu projektu dla którego chce się utworzyć odwołanie, w tym przypadku z katalogu ProductService.API
dotnet add reference ..\ProductService.Core\ProductService.Core.csproj
# Podobnie jak, powyżej, tyle że katalog projektu to ProductService.Core
dotnet add reference ..\ProductService.Infrastructure\ProductService.Infrastructure.csproj
dotnet new gitignore
```

- Warstwa prezentacji - `ProductService.API`
- Warstwa logiki biznesowej - `ProductService.Core`
- Wartswa dostępu do danych - `ProductService.Infrastructure`

- Artykuł: [dotnet new gitignore](https://dev.to/rafalpienkowski/easy-to-create-gitignore-for-the-dotnet-developers-1h42)