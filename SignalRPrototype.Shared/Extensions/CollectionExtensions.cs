namespace SignalRPrototype.Shared.Extensions;

public static class CollectionExtensions
{
    public static List<T> Merge<T>(this IEnumerable<T> input, IEnumerable<T> newValues, Func<T, T, bool> predicate)
    {
        List<T> result = new();
        foreach (var a in input)
        {
            var newValue = newValues.SingleOrDefault(b => predicate(a, b));
            result.Add(newValue ?? a);
        }
        return result;
    }
}