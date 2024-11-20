using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyCountManage : MonoBehaviour
{
    public static EnemyCountManage Instance { get; private set; }
    public List<EnemyController> activeEnemies = new List<EnemyController>();

    private float enemyDistance = 999999999.0f;

    private readonly object _lock = new object(); // Khóa để đảm bảo thread-safe

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Thêm quái
    public void AddEnemy(EnemyController enemy)
    {
        lock (_lock) // Đảm bảo không bị truy cập đồng thời
        {
            if (!activeEnemies.Contains(enemy))
            {
                Debug.Log("Add enemy");
                activeEnemies.Add(enemy);
            }
        }
    }

    // Xóa quái
    public void RemoveEnemy(EnemyController enemy)
    {
        lock (_lock) // Đảm bảo không bị truy cập đồng thời
        {
            if (activeEnemies.Contains(enemy))
            {
                activeEnemies.Remove(enemy);
            }
        }
    }


    public void SetEnemyDistance(float distance)
    {
        this.enemyDistance = distance;
    }

    public float GetEnemyDistance()
    {
        return this.enemyDistance;
    }

    public EnemyController GetClosestEnemy()
    {
        if (activeEnemies == null || activeEnemies.Count == 0)
        {
            return null; // Không có enemy nào để tìm
        }

        // Tìm enemy có distance nhỏ nhất
        return activeEnemies.OrderBy(enemy => enemy.distance).FirstOrDefault();
    }


}
