using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerBase : MonoBehaviour
{
    [Header("Save To Database")]
    public bool save = false;

    [Header("Player Information")]
    public Player player;

    private void Start()
    {
        if (player == null)
        {
            return;
        }

        // nếu saveToDatabase = true thì lưu player này vào database
        if (save)
        {
            // nếu player này chưa nhập vào id thì sẽ lấy id của gameObject, không thì sẽ lấy id mình nhập vào của item
            if (player.playerId == "")
            {
                player.playerId = gameObject.name;
            }

            // thêm player vào database
            StartCoroutine(PlayerManager_.Add(player));
        }
    }

  
}
