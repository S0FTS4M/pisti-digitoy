using System.Collections.Generic;

public static class Extensions
{
    public static void Shuffle<T>(this IList<T> list, int? seed = null)
    {
        System.Random rng;
        if (seed.HasValue == false)
            rng = new System.Random();
        else
        {
            rng = new System.Random(seed.Value);
        }
        int n = list.Count;

        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

}