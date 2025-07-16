public class XorshiftRandom : IRandom
{
    private uint state;

    public XorshiftRandom()
    {
        state = (uint)Environment.TickCount;

        if (state == 0) state = 2463534242u;
    }

    public XorshiftRandom(uint seed)
    {
        state = (seed != 0) ? seed : 2463534242u;
    }

    /// <summary>
    /// Generates a random uint
    /// </summary>
    public uint Next()
    {
        uint x = state;
        x ^= x << 13;
        x ^= x >> 17;
        x ^= x << 5;
        state = x;
        return x;
    }

     /// <summary>
    /// Returns a float in range [0.0f, 1.0f)
    /// </summary>
    public float Value()
    {
        return (Next() & 0xFFFFFF) / 16777216f; // 24-bit float precision
    }

    /// <summary>
    /// Returns a random int in range [min, max)
    /// </summary>
    public int Range(int min, int max)
    {
        if (min >= max)
            throw new ArgumentException("min must be less than max");
        return min + (int)(Next() % (uint)(max - min));
    }

    /// <summary>
    /// Returns a random float in range [min, max)
    /// </summary>
    public float Range(float min, float max)
    {
        if (min >= max)
            throw new ArgumentException("min must be less than max");
        return min + Value() * (max - min);
    }

    /// <summary>
    /// Returns true with the given probability (0.0 - 1.0)
    /// </summary>
    public bool Chance(float probability)
    {
        return Value() < probability;
    }
}