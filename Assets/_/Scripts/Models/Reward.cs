using System.Collections.Generic;

[System.Serializable]

public class Reward
{
    public float xp;                        // Số điểm kinh nghiệm nhận được
    public List<RewardItem> rewardItem;     // List các item có thể nhận được
}
