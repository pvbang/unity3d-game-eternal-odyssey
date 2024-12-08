<<<<<<< HEAD
﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemInfoDisplay : MonoBehaviour
{
    public GameObject itemInfoPanel; 
    public TextMeshProUGUI itemNameText;  
    public TextMeshProUGUI itemDescriptionText;  
    public TextMeshProUGUI itemTimeUsedText;

    private void Start()
    {
        itemInfoPanel.SetActive(false); 
    }

    public void ShowItemInfo(Item item)
    {
        itemNameText.text = item.name;
        itemDescriptionText.text = item.description;
        itemTimeUsedText.text = item.usingTime.ToString();
        itemInfoPanel.SetActive(true); 
    }

    public void HideItemInfo()
    {
        itemInfoPanel.SetActive(false);
    } 
}
=======
﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemInfoDisplay : MonoBehaviour
{
    public GameObject itemInfoPanel; 
    public TextMeshProUGUI itemNameText;  
    public TextMeshProUGUI itemDescriptionText;  
    public TextMeshProUGUI itemTimeUsedText;

    private void Start()
    {
        itemInfoPanel.SetActive(false); 
    }
    // Hàm để hiển thị thông tin item
    public void ShowItemInfo(Item item)
    {
        itemNameText.text = item.name;
        itemDescriptionText.text = item.description;
        itemTimeUsedText.text = item.usingTime.ToString();
        itemInfoPanel.SetActive(true); 
    }

    // Hàm để ẩn thông tin item
    public void HideItemInfo()
    {
        itemInfoPanel.SetActive(false);
    } 
}
>>>>>>> 8c5991d9d9de5b1fae16c33e06f7a66c81a6735d
