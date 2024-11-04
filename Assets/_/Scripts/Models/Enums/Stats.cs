[System.Serializable]

public class Stats
{
    public int current;
    public int max;

    public Stats()
    {
        current = 100;
        max = 100;
    }

    public Stats(int current, int max)
    {
        this.current = current;
        this.max = max;
    }
}
