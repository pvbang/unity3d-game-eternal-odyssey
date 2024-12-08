using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemPlayerWorld : MonoBehaviour
{
    public Image avatar;
    public TextMeshProUGUI namePlayer;

    public List<Sprite> sprites;

    public void SetData(Sprite avatar, string name)
    {
        this.avatar.sprite = avatar;
        this.namePlayer.text = name;
    }

    public void SetData(string name)
    {
        this.avatar.sprite = sprites[Random.Range(0, sprites.Count)];
        this.namePlayer.text = name;
    }

    public Sprite GetAvatar()
    {
        return avatar.sprite;
    }

    public string GetName()
    {
        return namePlayer.text;
    }
}
