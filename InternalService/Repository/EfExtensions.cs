using Microsoft.EntityFrameworkCore;

namespace InternalService.Repository;

public static class EfExtensions
{
    /// <summary>
    /// method for getting async list by Queryable in repositories e
    /// </summary>
    /// <param name="source"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Task<List<TSource>> ToListAsyncSafe<TSource>(
        this IQueryable<TSource> source)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (!(source is IAsyncEnumerable<TSource>))
            return Task.FromResult(source.ToList());
        return source.ToListAsync();
    }
}