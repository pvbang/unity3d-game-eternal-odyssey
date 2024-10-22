using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class UIItemAllWorld : MonoBehaviour
{
    public TextMeshProUGUI rankWorld;
    public TextMeshProUGUI nameWorld;
    public TextMeshProUGUI statusWorld;
    public TextMeshProUGUI startWorld;
    public TextMeshProUGUI endWorld;

    public void SetData(string rank, string name, string status, string start, string end)
    {
        rankWorld.text = rank;
        nameWorld.text = name;
        statusWorld.text = status;
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

    public void SetRank(string rank)
    {
        rankWorld.text = rank;
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
