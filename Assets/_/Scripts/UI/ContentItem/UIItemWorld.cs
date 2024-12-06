using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIItemWorld : MonoBehaviour
{
    public TextMeshProUGUI nameWorld;
    public TextMeshProUGUI statusWorld;
    public TextMeshProUGUI codeWorld;
    public TextMeshProUGUI startWorld;
    public TextMeshProUGUI endWorld;

    public void SetData(string name, string status, string code, string start, string end)
    {
        nameWorld.text = name;
        statusWorld.text = status;
        codeWorld.text = code;
        startWorld.text = start;
        endWorld.text = end;
    }

    public string GetName()
    {
        return nameWorld.text;
    }

    public string GetStatus()
    {
        return statusWorld.text;
    }

    public string GetCode()
    {
        return codeWorld.text;
    }

    public string GetStart()
    {
        return startWorld.text;
    }

    public string GetEnd()
    {
        return endWorld.text;
    }

    public void SetName(string name)
    {
        nameWorld.text = name;
    }

    public void SetStatus(string status)
    {
        statusWorld.text = status;
    }

    public void SetCode(string code)
    {
        codeWorld.text = code;
    }

    public void SetStart(string start)
    {
        startWorld.text = start;
    }

    public void SetEnd(string end)
    {
        endWorld.text = end;
    }
}
