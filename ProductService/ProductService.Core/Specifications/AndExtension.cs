namespace ProductService.Core.Specifications
{
    // Specyfika extension method wymaga aby klasa i metoda były statyczne, a rozszerzany typ
    // był pierwszym parametrem i zawieram słowo kluczowe this
    internal static class AndExtension
    {
        public static ISpecification<T> And<T>(this ISpecification<T> left, ISpecification<T> right)
        {
            return new AndSpecification<T>(left, right);
        }
    }
}
