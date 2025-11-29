using ProductService.Infrastructure.Entities;

namespace ProductService.Infrastructure.TestDataGenerator
{
    internal class ForbiddenWordGenerator
    {
        // To podejście ma też swoje niedociągnięcia, bo należałoby też walidować zakazane słowa
        // aczkolwiek to przede wszystkim wtedy kiedy aplikacja kontrolowałaby również dodawanie/modyfikację
        // zakazanych słów
        public static ForbiddenWord[] GetTestData()
        {
            return
            [
                new ForbiddenWord { Id = 1, Word = "python" },
                new ForbiddenWord { Id = 2, Word = "javascript" },
                new ForbiddenWord { Id = 3, Word = "mvc" }
            ];
        }
    }
}
