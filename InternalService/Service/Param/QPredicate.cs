using System.Linq.Expressions;

namespace InternalService.Service.Param;

public class QPredicate<T>
{
    private List<Func<T,bool>> _predicates;

    public QPredicate()
    {
        _predicates = new List<Func<T, bool>>();
    }

    public void Add(Func<T, bool> func)
    {
        _predicates.Add(t => func(t));
    }

    public Func<T, bool> Buid()
    {
        Func<T, bool> result = t => true;
        _predicates.ForEach(p =>
        {
            result = (t) => p(t) && result(t);
        });
        return result;
    }
}