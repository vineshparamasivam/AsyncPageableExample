
using System.Runtime.CompilerServices;

public abstract class AsyncPageable<T> : IAsyncEnumerable<T> where T : notnull
{
    public abstract IAsyncEnumerable<Page<T>> GetPagesAsync(string? continuationToken = default, int? pageSizeHint = default);

    public virtual async IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        await foreach(Page<T> page in GetPagesAsync().ConfigureAwait(false).WithCancellation(cancellationToken))
        {
            foreach(T item in page.Items)
            {
                yield return item;
            }
        }
    }

    private class StaticPageable : AsyncPageable<T>
    {
        private readonly IEnumerable<Page<T>> _pages;

        public StaticPageable(IEnumerable<Page<T>> pages)
        {
            _pages = pages;
        }

        public override async IAsyncEnumerable<Page<T>> GetPagesAsync(string? continuationToken = default, int? pageSizeHint = default)
        {
            var shouldReturn = continuationToken == null;

            foreach (Page<T> page in _pages)
            {
                if (shouldReturn)
                {
                    yield return page;
                }
                else if (page.ContinuationToken == continuationToken)
                {
                    shouldReturn = true;
                }
            }
        }
    }
}