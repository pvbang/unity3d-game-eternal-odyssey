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

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthText();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.fillAmount = currentHealth / maxHealth;
        UpdateHealthText();
    }

    public void Heal(float heal)
    {
        currentHealth += heal;
        healthBar.fillAmount = currentHealth / maxHealth;
        UpdateHealthText();
    }


    public void UpdateHealthText()
    {
        healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }
}
