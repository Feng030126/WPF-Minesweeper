public static class Extensions
{
    public static List<T> Sample<T>(this List<T> list, int count, List<T> exclude = null, IRandom rng = null)
    {
        rng ??= new XorshiftRandom();
        exclude ??= new();

        if (list.Count < count)
        {
            throw new IndexOutOfRangeException();
        }

        List<T> available = list.Where(e => !exclude.Contains(e)).ToList();

        int n = available.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Range(0, n + 1);
            (available[n], available[k]) = (available[k], available[n]);
        }

        return available.Take(count).ToList();
    }

}