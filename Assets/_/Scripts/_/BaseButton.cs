using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseButton : MonoBehaviour
{
    protected Button button;

    private void Start()
    {
        this.LoadButton();
        this.AddOnCLickEvent();
    }

    protected virtual void LoadButton()
    {
        if (button != null) return;
        button = GetComponent<Button>();
    }

    protected virtual void AddOnCLickEvent()
    {
        button.onClick.AddListener(this.OnClick);
    }

    protected abstract void OnClick();
}
