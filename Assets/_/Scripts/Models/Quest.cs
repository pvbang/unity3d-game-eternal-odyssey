using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Quest
{
    public string title;
    public string description;
    public List<string> reward;
    public string requirements;
    public string type;

    public Quest()
    {}

    public Quest(string title, string description, List<string> reward, string requirements, string type)
    {
        this.title = title;
        this.description = description;
        this.reward = reward;
        this.requirements = requirements;
        this.type = type;
    }
}