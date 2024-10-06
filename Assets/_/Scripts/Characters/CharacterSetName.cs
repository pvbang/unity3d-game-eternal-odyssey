using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterSetName : MonoBehaviour
{
    public CharacterDisplayName displayName;
    public TMP_InputField textName;

    private void Awake()
    {
        textName = GetComponent<TMP_InputField>();
    }

    public void SetName(string name)
    {
        displayName.UpdateName(name);
    }

    public void UpdateName()
    {
        displayName.UpdateName(textName.text);
    }
}
