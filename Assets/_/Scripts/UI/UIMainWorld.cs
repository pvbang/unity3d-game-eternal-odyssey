using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIMainWorld : MonoBehaviour
{
    public TMP_InputField nameWorld;
    public TMP_InputField codeWorld;

    public string GetNameWorld()
    {
        return nameWorld.text;
    }

    public string GetCodeWorld()
    {
        return codeWorld.text;
    }
}
