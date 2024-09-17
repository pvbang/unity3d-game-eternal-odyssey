using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    [Header("Reward Information")]
    public Reward reward;

    [HideInInspector]
    public List<string> itemIds = new();   // Danh sách itemId từ Firebase
}
