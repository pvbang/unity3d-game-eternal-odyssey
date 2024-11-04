using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pvbang : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerSystemController.instance.TakeDamage(100);
    }

}
