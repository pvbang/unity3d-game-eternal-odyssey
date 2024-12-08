using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Content : MonoBehaviour
{
    public Transform item;

    // xóa tất cả các item trong list
    public void DestroyContents()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    // tạo 1 item vào list
    public void AddContent(Transform content)
    {
        Instantiate(content, transform);
    }

    // tạo nhiều item vào list
    public void AddContent(List<Transform> contentList)
    {
        foreach (Transform content in contentList)
        {
            Instantiate(content, transform);
        }
    }

    // lấy item
    public Transform GetItem()
    {
        Transform item = this.item;
        return item;
    }
}