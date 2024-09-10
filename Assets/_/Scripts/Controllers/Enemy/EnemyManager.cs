using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public bool saveEnemyToFirebase = false;


    private void Start()
    {
        if (saveEnemyToFirebase)
        {
            AddEnemyFromEnemyList();
        }
    }

    public void CreateNewEnemy(Enemy enemy)
    {
        string json = JsonUtility.ToJson(enemy);
        string enemyId = gameObject.name;
        FirebaseManager.instance.databaseReference.Child("enemys").Child(enemyId).SetRawJsonValueAsync(json);
        Debug.Log("Enemy has been saved successfully!");
    }

    private void AddEnemyFromEnemyList()
    {
        EnemyList enemyList = gameObject.GetComponent<EnemyList>();
        Enemy enemy = enemyList.enemy;

        CreateNewEnemy(enemy);
    }
}
