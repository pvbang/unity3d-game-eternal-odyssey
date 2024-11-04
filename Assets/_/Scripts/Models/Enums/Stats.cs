[System.Serializable]

public class Stats
{
    public float current;
    public float max;

    public Stats()
    {
        current = 100f;
        max = 100f;
    }

    public Stats(float current, float max)
    {
        this.current = current;
        this.max = max;
    }
}
