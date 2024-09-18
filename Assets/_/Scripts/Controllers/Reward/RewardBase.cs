using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardBase : MonoBehaviour
{
    public Reward reward;

    [HideInInspector]
    public List<string> itemIds = new();   // Danh sách itemId từ Firebase

    // Lấy reward
    public Reward GetReward()
    {
        return this.reward;
    }

    // lấy danh sách rewardItem từ RewardManager
    public List<RewardItem> GetRewardItems()
    {
        return reward.rewardItem;
    }

    // Lấy rewardItem theo itemId
    public RewardItem GetRewardItem(string itemId)
    {
        return reward.rewardItem.Find(x => x.itemId == itemId);
    }

    // Thêm rewardItem mới
    public void AddRewardItem(RewardItem rewardItem)
    {
        reward.rewardItem.Add(rewardItem);
    }

    // Xóa rewardItem theo itemId
    public void RemoveRewardItem(string itemId)
    {
        reward.rewardItem.RemoveAll(x => x.itemId == itemId);
    }

    // Cập nhật rewardItem theo itemId
    public void UpdateRewardItem(string itemId, RewardItem rewardItem)
    {
        int index = reward.rewardItem.FindIndex(x => x.itemId == itemId);
        if (index >= 0)
        {
            reward.rewardItem[index] = rewardItem;
        }
    }
}
