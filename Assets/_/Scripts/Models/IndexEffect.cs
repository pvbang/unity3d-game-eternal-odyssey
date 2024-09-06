[System.Serializable]
public class IndexEffect
{
    public float rate;
    public float time;
    public int value;
    public IndexEffect()
    {
    }

    public IndexEffect(float rate, float time, int value)
    {
        this.rate = rate;
        this.time = time;
        this.value = value;
    }
}