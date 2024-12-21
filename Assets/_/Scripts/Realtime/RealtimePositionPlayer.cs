using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealtimePositionPlayer : MonoBehaviour
{
    public Transform realtimeObject;
    public float syncInterval = 0.3f; // Tần suất đồng bộ với server (200ms)

    private Vector3 previousPosition;
    private Vector3 previousRotation;
    private float syncTimer = 0f;

    private void Start()
    {
        if (realtimeObject == null)
        {
            realtimeObject = this.transform;
        }

        previousPosition = realtimeObject.localPosition;
        previousRotation = realtimeObject.localRotation.eulerAngles;
    }

    private void Update()
    {
        Vector3 position = realtimeObject.localPosition;
        Vector3 rotation = realtimeObject.localRotation.eulerAngles;

        if (position != previousPosition || rotation != previousRotation)
        {
            //ApplyLocalChanges(position, rotation);
            previousPosition = position;
            previousRotation = rotation;
        }

        syncTimer += Time.deltaTime;
        if (syncTimer >= syncInterval)
        {
            SyncWithServer();
            syncTimer = 0f;
        }
    }


    private void SyncWithServer()
    {
        PlayerSystemController.instance.UpdateLocation(previousPosition, previousRotation);
        //Debug.Log($"[Sync] Position sent to server: {previousPosition}, Rotation sent to server: {previousRotation}");
    }

    public void CorrectPosition(Vector3 serverPosition, Vector3 serverRotation)
    {
        // Smoothly adjust position and rotation to match server values
        realtimeObject.localPosition = Vector3.Lerp(realtimeObject.localPosition, serverPosition, Time.deltaTime * 10f);
        realtimeObject.localRotation = Quaternion.Lerp(realtimeObject.localRotation, Quaternion.Euler(serverRotation), Time.deltaTime * 10f);

        //Debug.Log($"[Correction] Correcting Position to: {serverPosition}, Rotation to: {serverRotation}");
    }
}
