namespace ProductService.Core.Specifications;

internal class AndSpecification<T> : ISpecification<T>
{
    private readonly ISpecification<T> _left;
    private readonly ISpecification<T> _right;
    private T? _lastCandidate;

    public AndSpecification(ISpecification<T> left, ISpecification<T> right)
    {
        _left = left;
        _right = right;
    }

    public bool IsSatisfiedBy(T candidate)
    {
        _lastCandidate = candidate;
        return _left.IsSatisfiedBy(candidate) && _right.IsSatisfiedBy(candidate);
    }

    public string GetErrorMessage()
    {
        if (_lastCandidate == null)
            return "IsSatisfiedBy must be called before GetErrorMessage";

        var errors = new List<string>();

        if (!_left.IsSatisfiedBy(_lastCandidate))
            errors.Add(_left.GetErrorMessage());

        if (!_right.IsSatisfiedBy(_lastCandidate))
            errors.Add(_right.GetErrorMessage());

        return string.Join("| ", errors);
    }
}