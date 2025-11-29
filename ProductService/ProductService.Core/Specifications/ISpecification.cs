namespace ProductService.Core.Specifications
{
    internal interface ISpecification<T>
    {
        bool IsSatisfiedBy(T entity);
        string GetErrorMessage();
    }
}