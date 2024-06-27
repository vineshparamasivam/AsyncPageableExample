public abstract class Page<T>{
    public abstract IReadOnlyList<T> Items { get; }

    public abstract string? ContinuationToken { get; }

    public static Page<T> FromValues(IReadOnlyList<T> values, string? continuationToken = null){
        return new PageCore(values, continuationToken);
    }

    private class PageCore : Page<T>
    {
        public override IReadOnlyList<T> Items { get; }

        public override string? ContinuationToken { get; }

        public PageCore(IReadOnlyList<T> items, string? continuationToken)
        {
            Items = items;
            ContinuationToken = continuationToken;
        }
    }
}



internal static class Page
{
    public static Page<T> FromValues<T>(IEnumerable<T> values, string continuationToken) =>
        Page<T>.FromValues(values.ToList(), continuationToken);
}