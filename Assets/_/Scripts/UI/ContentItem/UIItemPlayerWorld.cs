using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIItemPlayerWorld : MonoBehaviour
{
    public Sprite avatar;
    public TextMeshProUGUI namePlayer;

    public List<Sprite> sprites;

    public void SetData(Sprite avatar, string name)
    {
        this.avatar = avatar;
        this.namePlayer.text = name;
    }

    public void SetData(string name)
    {
        this.avatar = sprites[Random.Range(0, sprites.Count)];
        this.namePlayer.text = name;
    }

    public Sprite GetAvatar()
    {
        return avatar;
    }

    public string GetName()
    {
        return namePlayer.text;
    }
}
