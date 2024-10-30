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

    private void Awake()
    {
        if (isCheck)
        {
            checkImage.sprite = checkedImage;
            //skinObject.SetActive(true);
        }
        else
        {
            checkImage.sprite = uncheckImage;
            //skinObject.SetActive(false);
        }
    }

    private void UpdateUI()
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
        print("CharacterSkinButton OnClick");
        if (checkImage.sprite == checkedImage)
        {
            isCheck = false;
            checkImage.sprite = uncheckImage;
            UpdateUI();
        }
        else
        {
            isCheck = true; 
            checkImage.sprite = checkedImage;
            UpdateUI();
        }
    }
}
