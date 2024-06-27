
public class PageableHelpers
{
    public static AsyncPageable<T> CreateAsyncEnumerable<T>(Func<int?, Task<Page<T>>> firstPage, Func<string?, int?, Task<Page<T>>> nextPage, int? pageSize = default) where T : notnull
    {
        Func<string?, int?, Task<Page<T>>> first = (_, pageSize) => firstPage(pageSize);
         return new FuncAsyncPageable<T>(first, nextPage, pageSize);
    }

    
}

internal class FuncAsyncPageable<T> : AsyncPageable<T> where T : notnull{
    public FuncAsyncPageable(Func<string?, int?, Task<Page<T>>> firstPage, Func<string?, int?, Task<Page<T>>> nextPage, int? pageSize = default){
        _firstPage = firstPage;
        _nextPage = nextPage;
        _defaultPageSize = pageSize;
    }

    private readonly Func<string?, int?, Task<Page<T>>> _firstPage;
    private readonly Func<string?, int?, Task<Page<T>>> _nextPage;
    public readonly int? _defaultPageSize;

    public override async IAsyncEnumerable<Page<T>> GetPagesAsync(string? continuationToken = null, int? pageSizeHint = null)
    {
        Func<string?, int?, Task<Page<T>>>? pageFunc = string.IsNullOrEmpty(continuationToken) ? _firstPage : _nextPage;

        if (pageFunc == null)
        {
            yield break;
        }

         int? pageSize = pageSizeHint ?? _defaultPageSize;
        do
        {
            Page<T> pageResponse = await pageFunc(continuationToken, pageSize);
            yield return pageResponse;
            continuationToken = pageResponse.ContinuationToken;
            pageFunc = _nextPage;
        } while (!string.IsNullOrEmpty(continuationToken) && pageFunc != null);

    }
}