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

## Swagger UI

W celu implementacji swagger UI należało
1. Znajdując się w katalogu projektu ProductService.API dodać pakiet swashbuckle
   ```powershell
   dotnet add package Swashbuckle.AspNetCore
   ```
2. W pliku `program.cs` projektu ProductService.API dodać konfigurację:
   ```cs
   builder.Services.AddEndpointsApiExplorer();
   builder.Services.AddSwaggerGen();
   ```
   oraz
   ```cs
   if (app.Environment.IsDevelopment())
   {
      app.UseSwagger();
      app.UseSwaggerUI();
   }
   ```
3. Usunąć automatycznie wygenerowaną konfigurację builder i app dla OpenApi, ponieważ generuje ona błędy. Szczegóły usuniętych zmian można prześledzić w histori zmian repozytorium.

Ponadto dla konfiguracji `Development` skonfigurowane zostało przekierowanie z root `URL/` na `.../swagger`, można to łatwo wywnioskować z zawartości pliku `program.cs`

https://learn.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-8.0&tabs=visual-studio

## Zarządzanie Migracjami Bazy Danych

### Przywracanie stanu do wskazanej migracji

> Komenda została udokumentowana na wszelki wypadek, na szczęście na razie nie zaszła potrzeba skorzystania z niej.

```powershell
# pytanie czy należy dodać -p tak jak przy począkowej migracji?
dotnet ef database update [NAZWA_MIGRACJI] -s ProductService.API
```

### Usuwanie danej migracji

Aby usunąć ostatnie migracje, należy przywrócić stan do konkretnej migracji a następnie usunąć - **pytanie czy tylko ostatnią, czy wszystkie niezastosowane** - z użyciem niniejszej komendy

```powershell
dotnet-ef migrations remove -s ProductService.API -p ProductService.Infrastructure
```

### Tworzenie nowej migracji

Odbywa się tak samo jak tworzenie początkowej migracji.

```powershell
dotnet-ef migrations add UpdateProductAccordingToTaskDescription -p ProductService.Infrastructure -s ProductService.API
```

### Wypisanie listy migracji

```powershell
dotnet-ef migrations list -p ProductService.Infrastructure -s ProductService.API
```

### Wykonanie migracji

Przeprowadzane jest identycznie jak dla początkowej migracji.

```powershell
dotnet-ef database update -p ProductService.Infrastructure -s ProductService.API
```

https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/managing?tabs=dotnet-core-cli

## Materiały Wykorzystane do Implementacji Rozwiązania

- https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/records
- https://learn.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-10.0#binding-source-parameter-inference
- https://learn.microsoft.com/en-us/ef/core/querying/tracking - W skrócie non-tracking queries tylko dla read-only. Tracking dla pozostałych.
- https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection#service-lifetimes - Cykle życia: transient, scoped, singleton.
- https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying?tabs=dotnet-core-cli#apply-migrations-at-runtime - Aplikowanie migracji wewnątrz kodu aplikacji.
- https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-10.0#middleware-order - Kolejność middleware (ważna kwestia!)

## Problem ze swaggerem po implementacji kontrolera/serwisów/repozytoriów

W projekcie `ProductService.API` w trakcie wykonywania *app.MapControllers();* rzucany był wyjątek związany z niemożnością dynamicznego zalinkowania bibliotek związanych z OpenAPI.

Pomogło usunięcie ówczesnej wersji zależności `Swashbuckle` i downgrade do niższej wersji:

```powershell
# O dziwo ta wersja działa w przeciwieństwie do 10.x
dotnet add package Swashbuckle.AspNetCore --version 6.9.0
```