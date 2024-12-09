using System.Collections;
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
