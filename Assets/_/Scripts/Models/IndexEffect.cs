[System.Serializable]
public class IndexEffect
{
    public float rate;
    public float time;
    public Attribute attribute;
    public IndexEffect()
    {
    }

    public IndexEffect(float rate, float time, Attribute attribute)
    {
        this.rate = rate;
        this.time = time;
        this.attribute = attribute;
    }
}