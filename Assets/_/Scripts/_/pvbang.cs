using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pvbang : MonoBehaviour
{
    void Start()
    {
        ItemManager.GetItemById("go_cung", item => { Debug.Log(item.properties); });

    }

}
