using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestInteractionHandler : MonoBehaviour
{
    public string chestId;
    public GameObject hintUI;
    public static bool isPlayerInRange = false;
    private GameObject hintInstance;
    private void Start()
    {
        if (hintUI != null)
        {
            hintInstance = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "HintChest" && other.CompareTag(ConstantManager.PLAYER_TAG))
        {
            isPlayerInRange = true;

            GameObject chestHint = GameObject.FindGameObjectWithTag("ChestHint");

            if (hintUI != null && chestHint != null)
            {
                if (hintInstance == null)
                {
                    hintInstance = Instantiate(hintUI, chestHint.transform); 
                    hintInstance.SetActive(true);
                    Debug.Log("Gợi ý đã được hiển thị.");
                }
            }
            Debug.Log($"Người chơi đã vào vùng rương {chestId}");
            InventoryManager.Instance.SetCurrentChest(this);
            chestId = gameObject.name;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "HintChest" && other.CompareTag(ConstantManager.PLAYER_TAG))
        {
            isPlayerInRange = false;

            if (hintInstance != null)
            {
                hintInstance.SetActive(false);
                Destroy(hintInstance);
                hintInstance = null;    
            }
            InventoryManager.Instance?.ClearCurrentChest(this);
            Debug.Log($"Người chơi đã rời vùng rương {chestId}");
        }
    }

    public void TryOpenChest()
    {
        if (!isPlayerInRange)
        {
            Debug.LogWarning("Người chơi không ở gần rương.");
            return;
        }
        var groundItemUI = FindObjectOfType<GroundItemUI>();
        if (groundItemUI != null)
        {
            groundItemUI.OnChestOpened(chestId);
        }
        else
        {
            Debug.LogError("Không tìm thấy GroundItemUI trong scene.");
        }
    }
}
