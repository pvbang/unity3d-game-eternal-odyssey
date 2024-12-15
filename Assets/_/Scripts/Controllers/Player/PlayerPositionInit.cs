using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerPositionInit : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(DisplayLocation());
    }

    public IEnumerator DisplayLocation()
    {
        string worldId = PlayerPrefs.GetString(ConstantManager.FIREBASE_WORLDS_ID);
        string userId = FirebaseManager.instance.GetUserId();

        PlayerWorld playerWorld = new();
        bool checkCallback = false;

        WorldManager.GetPlayerLocation(worldId, userId, _value =>
        {
            playerWorld = _value;
            checkCallback = true;
        });

        yield return new WaitUntil(() => checkCallback);

        if (playerWorld == null)
        {
            yield break;
        }

        SetLocation(playerWorld);
    }

    public void SetLocation(PlayerWorld playerWorld)
    {
        Vector3 defaultPosition = new Vector3(playerWorld.position.x, playerWorld.position.y, playerWorld.position.z);
        Vector3 defaultRotation = new Vector3(playerWorld.rotation.x, playerWorld.rotation.y, playerWorld.rotation.z);

        transform.localPosition = defaultPosition;
        transform.localRotation = Quaternion.Euler(defaultRotation);
    }
}
