using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonActive : BaseButton
{
    private bool isActive = false;

    [Header("Click => Active => Click => Deactive => Loop")]
    public bool isReverseButton = true;

    public Transform objectActive;


    override protected void OnClick()
    {
        if (isReverseButton)
        {
            objectActive.gameObject.SetActive(isActive);
            isActive = !isActive;
        }
        else
        {
            isActive = true;
            objectActive.gameObject.SetActive(isActive);
        }
    }
}
