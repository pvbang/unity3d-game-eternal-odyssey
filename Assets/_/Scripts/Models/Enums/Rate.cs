[System.Serializable]

public class Rate
{
    public float from;
    public float to;

    public Rate()
    {
        from = 0f;
        to = 0f;
    }

    public Rate(float from, float to)
    {
        this.from = from;
        this.to = to;
    }
}
