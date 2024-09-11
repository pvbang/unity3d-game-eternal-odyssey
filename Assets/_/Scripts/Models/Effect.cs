[System.Serializable]

public class Effect
{
    public string name;             
    public EffectType type;             // Loại hiệu ứng (Health, Damage, Resistance, AttackSpeed, RunSpeed, Intelligence, Crit)
    public string description;
    public float rate;   // (%)         // Tỉ lệ kích hoạt động của hiệu ứng                
    public float time;   // (giây)      // Thời gian tồn tại của hiệu ứng                  
    public Attribute playerAttribute;   // Thuộc tính của player sau khi áp dụng hiệu ứng
    public Attribute enemyAttribute;    // Thuộc tính của enemy sau khi áp dụng hiệu ứng

    public Effect()
    {
    }

    public Effect(string name, EffectType type, string description, float rate, float time, Attribute playerAttribute, Attribute enemyAttribute)
    {
        this.name = name;
        this.type = type;
        this.description = description;
        this.rate = rate;
        this.time = time;
        this.playerAttribute = playerAttribute;
        this.enemyAttribute = enemyAttribute;
    }

}