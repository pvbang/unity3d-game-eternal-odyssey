using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSkinButton : BaseButton
{
    public bool isCheck = false;
    public Sprite checkedImage;
    public Sprite uncheckImage;

    public Image checkImage;
    public GameObject skinObject;

    private void Update()
    {
        if (skinObject == null) return;

        if (isCheck)
        {
            skinObject.SetActive(true);
        }
        else
        {
            skinObject.SetActive(false);
        }
    }

    protected override void OnClick()
    {
        if (checkImage.sprite == checkedImage)
        {
            isCheck = false;
            checkImage.sprite = uncheckImage;
        }
        else
        {
            isCheck = true; 
            checkImage.sprite = checkedImage;
        }
    }
}
