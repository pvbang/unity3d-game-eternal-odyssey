using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using System.IO;
public class WeaponManager : MonoBehaviour
{
    public bool saveWeaponToFirebase = false;

    DatabaseReference databaseReference;

    void Awake()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private void Start()
    {
        if(saveWeaponToFirebase) {
            AddWeaponFromWeaponList();
        }
    }

    public void CreateNewWeapon(Weapon weapon)
    {
        string json = JsonUtility.ToJson(weapon);
        string weaponId = gameObject.name;
        databaseReference.Child("weapons").Child(weaponId).SetRawJsonValueAsync(json);
        Debug.Log("Item has been saved successfully!");
    }

    private void AddWeaponFromWeaponList()
    {
        WeaponList weaponList = gameObject.GetComponent<WeaponList>();
        Weapon weapon = weaponList.weapon;

        CreateNewWeapon(weapon);
    }
}
