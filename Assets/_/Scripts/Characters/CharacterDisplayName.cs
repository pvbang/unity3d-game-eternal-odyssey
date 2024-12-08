using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDisplayName : MonoBehaviour
{
    public Text textName;

    private void Awake()
    {
        textName = GetComponent<Text>();
    }

    // update text name
    public void UpdateName(string name)
    {
        textName.text = name;
    }
}
