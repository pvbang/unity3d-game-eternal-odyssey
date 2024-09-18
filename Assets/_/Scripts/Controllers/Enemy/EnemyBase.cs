using UnityEngine;

[System.Serializable]
public class EnemyBase : MonoBehaviour
{
    [Header("Save To Database")]
    public bool save = false;

    [Header("Item Information")]
    public Enemy enemy;

    private RewardManager rewardManager;

    private void Awake()
    {
        rewardManager = gameObject.GetComponent<RewardManager>();
    }

    private void Start()
    {
        if (enemy == null)
        {
            return;
        }

        // nếu saveToDatabase = true thì lưu enemy này vào database
        if (save)
        {
            // nếu enemy này chưa nhập vào id thì sẽ lấy id của gameObject, không thì sẽ lấy id mình nhập vào của item
            if (enemy.enemyId == "")
            {
                enemy.enemyId = gameObject.name;
            }

            // nếu rewardManager khác null thì sẽ lấy reward từ rewardManager
            if (rewardManager != null)
            {
                enemy.reward = rewardManager.GetReward();
            }

            // thêm enemy vào database
            StartCoroutine(EnemyManager.Add(enemy));
        }
    }
}
