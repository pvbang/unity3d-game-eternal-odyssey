using UnityEngine;

public class EnemyDamageHandle : MonoBehaviour
{
    private EnemyAtribute enemyAtribute;

    private void Start()
    {
        enemyAtribute = GetComponent<EnemyAtribute>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ConstantManager.SKILL_TAG) || other.CompareTag(ConstantManager.WEAPON_TAG))
        {
            TakeDamageValue takeDamageValue = other.GetComponent<TakeDamageValue>();
            if (takeDamageValue != null) TakeDamage(takeDamageValue);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(ConstantManager.SKILL_TAG) || collision.gameObject.CompareTag(ConstantManager.WEAPON_TAG))
        {
            TakeDamageValue takeDamageValue = collision.gameObject.GetComponent<TakeDamageValue>();
            if (takeDamageValue != null) TakeDamage(takeDamageValue);
        }
    }

    private void TakeDamage(TakeDamageValue takeDamageValue)
    {
        int damage = takeDamageValue.GetDamage() - enemyAtribute.getResistance();
        Debug.Log($"Enemy took {damage} damage!");
        
        if (damage < 0) return;

        enemyAtribute.setCurrentHealth(enemyAtribute.getCurrentHealth() - damage);
        if (enemyAtribute.getCurrentHealth() <= 0)
        {
            Destroy(gameObject);
        }
    }
}
