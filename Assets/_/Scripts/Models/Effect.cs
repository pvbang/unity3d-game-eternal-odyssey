using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class Effect
{
    public string name;
    public List<IndexEffect> list_index_effect_id;
    public Effect()
    {
    }

    public Effect(string name, List<IndexEffect> list_index_effect_id)
    {
        this.name = name;
        this.list_index_effect_id = list_index_effect_id;
    }
}