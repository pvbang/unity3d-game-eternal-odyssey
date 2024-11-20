using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAtributeUI : MonoBehaviour
{
    public static EnemyAtributeUI Instance;

    public GameObject EnemyHP;
    private HealthBarUI healthBarUI;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        if (healthBarUI == null)
        {
            healthBarUI = EnemyHP.GetComponentInChildren<HealthBarUI>();
        }
    }

    
    public void SetActiveEnemyHP()
    {
        EnemyHP.SetActive(true);
    }

    public void SetDeactiveEnemyHP()
    {
        EnemyHP.SetActive(false);
    }

    public void UpdateEnemyHP(float currentHealth, float maxHealth)
    {
        healthBarUI.UpdateHealth(currentHealth, maxHealth);
    }

    public void FixedUpdate()
    {
        EnemyController enemy = EnemyCountManage.Instance.GetClosestEnemy();
        if (enemy != null)
        {
            if (enemy.distance < enemy.sightRange)
            {
                SetActiveEnemyHP();
                UpdateEnemyHP(enemy.enemyAtribute.getCurrentHealth(), enemy.enemyAtribute.getMaxHealth());
            }
            else
            {
                SetDeactiveEnemyHP();
            }
        }
    }

}
