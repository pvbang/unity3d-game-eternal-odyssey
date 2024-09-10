using UnityEngine;
using Firebase.Database;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager instance;

    public DatabaseReference databaseReference;

    private void Awake()
    {
        FirebaseManager.instance = this;
        this.databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }
}
