using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryPanel;

    private bool isOpenInventory = false;
    // Update is called once per frame
    private void Start()
    {
        // Ẩn Inventory khi bắt đầu
        inventoryPanel.SetActive(false);

        // Gán sự kiện cho nút CloseButton
    }

    private void Update()
    {
        // Kiểm tra xem người dùng có nhấn phím 'I' không
        if (Input.GetKeyDown(KeyCode.I))
        {
            // Chuyển đổi trạng thái Inventory
            isOpenInventory = !isOpenInventory;
            inventoryPanel.SetActive(isOpenInventory);
        }
    }
}
