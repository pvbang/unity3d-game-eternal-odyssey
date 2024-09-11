using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Extensions;
using Firebase.Database;

public class FirebaseInit : MonoBehaviour
{
    void Start()
    {
        FirebaseDatabase.DefaultInstance.GetReference(".info/connected").ValueChanged += (object sender, ValueChangedEventArgs args) =>
        {
            if (args.DatabaseError != null)
            {
                Debug.LogError(args.DatabaseError.Message);
                return;
            }
            Debug.Log("Firebase connection status: " + (args.Snapshot.Value.ToString() == "True" ? "Connected" : "Disconnected"));
        };
    }

}
