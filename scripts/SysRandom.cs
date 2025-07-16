public class SysRandom : IRandom
{
    System.Random rng;

    public SysRandom()
    {
        rng = new Random();
    }

    public SysRandom(uint seed)
    {
        rng = new Random((int)seed);
    }

    public uint Next()
    {
        throw new NotImplementedException();
    }

    public float Range(float min, float max)
    {
        throw new NotImplementedException();
    }

    public int Range(int min, int max)
    {
        throw new NotImplementedException();
    }
}