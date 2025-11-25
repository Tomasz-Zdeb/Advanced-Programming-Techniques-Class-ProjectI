# Projekt 1 - Tomasz Zdeb

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

## Inicjalizacja Bazy Danych

Baza danych wykorzystana do development to [SQL Server Express w kontenerze OCI](https://learn.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker?view=sql-server-ver17&tabs=cli&pivots=cs1-bash). Niestandardowy obraz silnika bazy danych nie jest tworzony, zamiast tego w ramach projektu wykorzystany jest bezpośrednio obraz dostarczany przez Microsoft. Uruchomienie kontenera można przeprowadzić m.in. za pomocą pliku `docker-compose.yml` dołączonego do repozytorium. Oznacza to, że za każdym razem dla projektu tworzona jest nowa instancja bazy danych, która zostaje wypełniona syntetycznymi danymi testwymi w jednej z migracji (w rozumieniu *Entity Framework*)

Komunikacja serwisu z bazą danych została zaimplementowana przy pomocy najpopularniejszego **ORM** w ekosystemie .NET, tj **Entity Framework** (EF).

Aby wykonywać polecenia *Entity Framework* konieczna jest również instalacja CLI. W moim przypadku dokonana została instalacja globalna, ważne było wskazanie wersji, w przeciwnym wypadku operacja kończyła się niepowodzeniam

```powershell
dotnet tool install --global dotnet-ef --version 9.0.0
```

Konieczne jest również dodanie pakietów NuGet związanych z EF do odpowiednich projektów - należy dodać parametr określający wersję. Gdy tego nie zrobiłem, otrzymałem informację o niekompatybilnej wersji.
   ```powershell
   # Uwaga - pakiety dołącza się do konktretnego projektu, więc należy zmienić katalog na katalog projektu warstwy dostępu do danych (Infrastructure).
   dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 9.0.0
   dotnet add package Microsoft.EntityFrameworkCore.Design --version 9.0.0
   # Dla projektu API również, ze względu, że będzie to startup
   # project
   dotnet add package Microsoft.EntityFrameworkCore.Design --version 9.0.0
   ```

Utworzenie początkowej migracji zawierającej uproszczony model produktu wraz z kodem generującym testowe wartości przeprowadzone zostało za pomocą komendy:

```powershell
dotnet-ef migrations add InitialCreate -p ProductService.Infrastructure -s ProductService.API
```

https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli

Następnie aby aplikować migrację należy wykonać komendę:

```powershell
dotnet-ef database update -p ProductService.Infrastructure -s ProductService.API
```



## Konfiguracja Połączenia Serwisu z Bazą Danych

1. Konfiguracja lokalnego sekretu - wzorowane na artykule:
   https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-10.0&tabs=windows
   ```powershell
   # Należy wykonać w katalogu projektu ProductService.API jeżeli
   # lokalna przechowalnia sekretów nie była inicjowana
   dotnet user-secrets init
   # Oczywiście komendy na ustawienie sekretu produkcyjnego pod
   # żadnym pozorem nie powinno się umieszczać w repozytorium
   # w przypadku testowej bazy w ramach projektu edukacyjnego
   # jest to dopuszczalne
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,1433;Database=ProductDB;User Id=sa;Password=ProductServiceDevelopmentDb1!;TrustServerCertificate=True;"
   ```
2. Pobranie wartości connection stringa przy konfiguracji aplikacji, tj w pliku `Program.cs` prjektu **ProductService.API**.

   ```csharp
   builder.Services.AddDbContext<ApplicationDbContext>(options =>
      options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
   ```

## Uruchomienie Silnika Bazy Danych

Wykonanie pliku `docker-compose.yml` przeprowdza się za pomocą komendy `docker-compose up`, można dodać opcjonalnie flagę `-d` (od *detached*) aby proces został uruchomiony w tle i nie blokował okna terminala.

W celu wyłączenia projektu zdefiniowanego w `docker-compose.yml` wykorzystuje się komendę `docker-compose down`

Przy zastosowaniu podejścia opartego o narzędzie `docker-compose`, należy być świadomym utworzenia `volume` dla kontenera bazy. W celu przeprowadzenia pełnego testu *white room*, należy usunąć istniejący `volume`.