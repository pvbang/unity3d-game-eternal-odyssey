using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class Effect
{
    public string description;
    public List<IndexEffect> list_index_effect_id;
    public Effect()
    {
    }

    public Effect(string description, List<IndexEffect> list_index_effect_id)
    {
        this.description = description;
        this.list_index_effect_id = list_index_effect_id;
    }
}