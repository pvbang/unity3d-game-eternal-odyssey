[System.Serializable]

public class Rate
{
    public int from;
    public int to;

    public Rate()
    {
        from = 0;
        to = 0;
    }

    public Rate(int from, int to)
    {
        this.from = from;
        this.to = to;
    }
}
