using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Cách sử dụng: WaitingManager.instance.ShowWaiting();
public class WaitingManager : MonoBehaviour
{
    public static WaitingManager instance;
    public GameObject waitingObject;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        HideWaiting();
    }

    public void ShowWaiting()
    {
        waitingObject.SetActive(true);
    }

    public void HideWaiting()
    {
        waitingObject.SetActive(false);
    }
}
