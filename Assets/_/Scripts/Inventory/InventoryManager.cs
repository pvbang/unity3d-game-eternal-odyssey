using NullSave.TOCK.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryPanel;
    public static InventoryManager Instance;

    public static bool isOpenInventory = false;
    public GroundItemUI groundItemUI;
    public GroundItemSystem groundSystem;

    private ChestInteractionHandler currentChest;
    private List<ChestInteractionHandler> chestInteractionHandlers;

    private void Start()
    {
        inventoryPanel.SetActive(false);
        chestInteractionHandlers = new List<ChestInteractionHandler>(FindObjectsOfType<ChestInteractionHandler>());
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("InventoryManager.Instance đã tồn tại!");
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isOpenInventory = !isOpenInventory;
            inventoryPanel.SetActive(isOpenInventory);

            if (isOpenInventory)
            {
                groundItemUI.UpdateGroundUI(groundSystem.GetGroundItems());
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                CloseInventory();
                if (currentChest != null)
                {
                    currentChest.TryOpenChest();
                }
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                OpenInventory();

                var groundItemSystem = FindObjectOfType<GroundItemSystem>();
                if (groundItemSystem != null)
                {
                    groundItemSystem.ClearAllItems();
                }
                else
                {
                    Debug.LogError("Không tìm thấy groundItemSystem trong scene.");
                }
            }
        }
    }
    private void OpenInventory()
    {
        foreach (var chest in chestInteractionHandlers)
        {
            if (chest.hintUI != null)
            {
                chest.hintUI.SetActive(false);
            }
        }
    }

    private void CloseInventory()
    {
        foreach (var chest in chestInteractionHandlers)
        {
            if (chest.hintUI != null)
            {
                chest.hintUI.SetActive(true);
            }
        }
    }
    public void SetCurrentChest(ChestInteractionHandler chest)
    {
        currentChest = chest;
    }

    public void ClearCurrentChest(ChestInteractionHandler chest)
    {
        if (currentChest == chest)
        {
            currentChest = null;
        }
    }
}
