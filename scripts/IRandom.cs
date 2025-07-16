public interface IRandom
{
    public uint Next();

    public float Range(float min, float max);

    public int Range(int min, int max);
}