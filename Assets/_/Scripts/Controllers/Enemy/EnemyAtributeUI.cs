using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAtributeUI : MonoBehaviour
{
    public static EnemyAtributeUI instance;

    public GameObject EnemyHP;
    private HealthBarUI healthBarUI;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
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


}
