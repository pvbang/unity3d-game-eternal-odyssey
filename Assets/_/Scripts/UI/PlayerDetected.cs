using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetected : MonoBehaviour
{
    public bool spawnWhenDetected = true;

    public RealtimeObjectSpawn realtimeObjectSpawn;

    private void Start()
    {
        if (realtimeObjectSpawn == null)
        {
            realtimeObjectSpawn = GetComponent<RealtimeObjectSpawn>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ConstantManager.PLAYER_TAG))
        {
            if (spawnWhenDetected)
            {
                realtimeObjectSpawn.SpawnObject();
            }
        }
    }
}
