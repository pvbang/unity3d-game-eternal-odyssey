using UnityEngine;

[System.Serializable]
public class EnemyList : MonoBehaviour
{
    [Header("Save To Database")]
    public bool save = false;

    [Header("Item Information")]
    public Enemy enemy;

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
            // thêm enemy vào database
            StartCoroutine(EnemyManager.AddEnemy(enemy));
        }
    }
}
