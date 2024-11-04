using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Image healthBar;
    public Text healthText;

    public float maxHealth = 1000f;
    public float currentHealth;

    private void Awake()
    {
        if (healthBar == null)
        {
            GameObject healthBarObj = transform.Find("HealthBar").gameObject;
            healthBar = healthBarObj.GetComponent<Image>();
        }
        if (healthText == null)
        {
            GameObject healthTextObj = transform.Find("HealthText").gameObject;
            healthText = healthTextObj.GetComponent<Text>();
        }
    }

    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        this.currentHealth = currentHealth;
        this.maxHealth = maxHealth;

        healthBar.fillAmount = currentHealth / maxHealth;
        UpdateHealthText();
    }

    // Nhận sát thương => giảm máu
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.fillAmount = currentHealth / maxHealth;
        UpdateHealthText();
    }

    // Hồi máu
    public void Heal(float heal)
    {
        currentHealth += heal;
        healthBar.fillAmount = currentHealth / maxHealth;
        UpdateHealthText();
    }


    // Cập nhật text hiển thị máu
    public void UpdateHealthText()
    {
        healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }


    // Buff tăng máu tạm thời trong một khoảng thời gian
    public void BuffHealth(float buffHealth, float duration)
    {
        StartCoroutine(BuffHealthCoroutine(buffHealth, duration));
    }

    private IEnumerator BuffHealthCoroutine(float buffHealth, float duration)
    {
        float originalMaxHealth = maxHealth;
        maxHealth += buffHealth;
        currentHealth += buffHealth;
        healthBar.fillAmount = currentHealth / maxHealth;
        UpdateHealthText();

        yield return new WaitForSeconds(duration);

        maxHealth = originalMaxHealth;
        healthBar.fillAmount = currentHealth / maxHealth;
        UpdateHealthText();
    }

    // Buff tăng máu vĩnh viễn
    public void BuffMaxHealth(float buffHealth)
    {
        maxHealth += buffHealth;
        currentHealth += buffHealth;
        healthBar.fillAmount = currentHealth / maxHealth;
        UpdateHealthText();
    }

    // Debuff giảm máu vĩnh viễn
    public void DebuffMaxHealth(float debuffHealth)
    {
        maxHealth -= debuffHealth;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        healthBar.fillAmount = currentHealth / maxHealth;
        UpdateHealthText();
    }

    // Debuff giảm máu tạm thời trong một khoảng thời gian
    public void DebuffHealth(float debuffHealth, float duration)
    {
        StartCoroutine(DebuffHealthCoroutine(debuffHealth, duration));
    }

    private IEnumerator DebuffHealthCoroutine(float debuffHealth, float duration)
    {
        float originalMaxHealth = maxHealth;
        maxHealth -= debuffHealth;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        healthBar.fillAmount = currentHealth / maxHealth;
        UpdateHealthText();

        yield return new WaitForSeconds(duration);

        maxHealth = originalMaxHealth;
        healthBar.fillAmount = currentHealth / maxHealth;
        UpdateHealthText();
    }

    // Bất tử trong một khoảng thời gian
    public void Immortal(float duration)
    {
        StartCoroutine(ImmortalCoroutine(duration));
    }

    private IEnumerator ImmortalCoroutine(float duration)
    {
        float originalMaxHealth = maxHealth;
        maxHealth = Mathf.Infinity;
        healthBar.fillAmount = currentHealth / maxHealth;
        UpdateHealthText();

        yield return new WaitForSeconds(duration);

        maxHealth = originalMaxHealth;
        healthBar.fillAmount = currentHealth / maxHealth;
        UpdateHealthText();
    }

}
