using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;

public class UserManager : MonoBehaviour
{
    DatabaseReference databaseReference;

    void Awake()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private void Start()
    {
        AddInitUser();
    }

    private void WriteNewUser(string username, string email)
    {
        User user = new(username, email);
        string json = JsonUtility.ToJson(user);

        string userId = RandomManager.GenerateRandomID();
        databaseReference.Child("users").Child(userId).SetRawJsonValueAsync(json);
    }
   
    private void AddInitUser()
    {
        string username = "tai day";
        string email = "la@gmail.com";

        WriteNewUser(username, email);
    }    
}
