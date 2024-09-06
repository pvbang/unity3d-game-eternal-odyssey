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
        addiInitUser();
    }

    private void writeNewUser(string username, string email)
    {
        User user = new User(username, email);
        string json = JsonUtility.ToJson(user);

        string userId = RandomManager.GenerateRandomID();
        databaseReference.Child("users").Child(userId).SetRawJsonValueAsync(json);
    }

    private void addiInitUser()
    {
        string username = "huan";
        string email = "la@gmail.com";

        writeNewUser(username, email);
    }
}
