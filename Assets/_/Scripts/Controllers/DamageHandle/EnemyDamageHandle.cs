using UnityEngine;

public class EnemyDamageHandle : MonoBehaviour
{
    public EnemyAtribute enemyAtribute;
    public EnemyAnimationController enemyAnimationController;


    private void Start()
    {
        if (enemyAtribute == null) enemyAtribute = GetComponent<EnemyAtribute>();
        if (enemyAnimationController == null) enemyAnimationController = GetComponent<EnemyAnimationController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ConstantManager.PLAYER_SKILL_TAG) || other.CompareTag(ConstantManager.WEAPON_TAG))
        {
            TakeDamageValue takeDamageValue = other.GetComponent<TakeDamageValue>();
            if (takeDamageValue != null) TakeDamage(takeDamageValue);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(ConstantManager.PLAYER_SKILL_TAG) || collision.gameObject.CompareTag(ConstantManager.WEAPON_TAG))
        {
            TakeDamageValue takeDamageValue = collision.gameObject.GetComponent<TakeDamageValue>();
            if (takeDamageValue != null) TakeDamage(takeDamageValue);
        }
    }

    private void TakeDamage(TakeDamageValue takeDamageValue)
    {
        int damage = takeDamageValue.GetDamage() - enemyAtribute.getResistance();
        Debug.Log($"Enemy took {damage} damage!");
        
        if (damage < 0) damage = 1;

        enemyAnimationController.PlayAnimation("get_hit");

        int currentHealth = enemyAtribute.getCurrentHealth() - damage;
        if (currentHealth < 0) currentHealth = 0;
        enemyAtribute.setCurrentHealth(currentHealth);
        if (enemyAtribute.getCurrentHealth() <= 0)
        {
            enemyAnimationController.PlayAnimation("die");
            Destroy(gameObject, 5f);
        }
    }
}
