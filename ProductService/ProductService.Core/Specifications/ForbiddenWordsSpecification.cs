using ProductService.Infrastructure.Entities;
using ProductService.Infrastructure.Repositories;

namespace ProductService.Core.Specifications
{
    internal class ForbiddenWordsSpecification : ISpecification<Product>
    {
        private readonly IForbiddenWordRepository _forbiddenWordRepository;
        private string? _violatedWord;

        public ForbiddenWordsSpecification(IForbiddenWordRepository forbiddenWordRepository)
        {
            _forbiddenWordRepository = forbiddenWordRepository;
        }

        public bool IsSatisfiedBy(Product validatedObject)
        {
            _violatedWord = null;
            var forbiddenWords = _forbiddenWordRepository.GetAllAsync().Result;

            foreach (var forbiddenWord in forbiddenWords)
            {
                if (validatedObject.Name.Contains(forbiddenWord.Word, StringComparison.OrdinalIgnoreCase))
                {
                    _violatedWord = forbiddenWord.Word;
                    return false;
                }
            }

            return true;
        }

        public string GetErrorMessage()
        {
            return $"Product name contains forbidden word: '{_violatedWord}'";
        }
    }
}
