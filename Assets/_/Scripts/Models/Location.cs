[System.Serializable]
public class Location
{
    public float x;
    public float y;
    public float z;

    public Location()
    {
        x = 0;
        y = 0;
        z = 0;
    }

    public Location(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public float X { get => x; set => x = value; }
    public float Y { get => y; set => y = value; }
    public float Z { get => z; set => z = value; }
}
