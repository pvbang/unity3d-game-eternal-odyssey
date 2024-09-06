[System.Serializable]
public class Skills
{
   public string name;
   public int damage;
   public Skills()
   {}

   public Skills(string name, int damage)
   {
    this.name = name;
    this.damage = damage;
   }
}
