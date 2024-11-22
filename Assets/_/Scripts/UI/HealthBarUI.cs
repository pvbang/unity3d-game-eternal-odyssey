using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Image healthBar;
    public Text healthText;

    public GameObject damageTextObject;
    public Text damageText;

    public float maxHealth = 1000f;
    public float currentHealth;

    private Coroutine activeCoroutine;


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

    public void UpdateTextDamage(string damage, Color color)
    {
        // Kiểm tra nếu damageTextObject đang active thì deactivate và activate lại
        if (damageTextObject.activeSelf)
        {
            damageTextObject.SetActive(false);
        }

        // Cập nhật text và màu
        damageText.text = damage;
        damageText.color = color;

        // Kích hoạt lại damageTextObject
        damageTextObject.SetActive(true);

        // Nếu đã có coroutine đang chạy thì dừng lại
        if (activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);
        }

        // Chạy coroutine để deactivate sau 1s
        activeCoroutine = StartCoroutine(DeactivateAfterDelay(1.2f));
    }

    private IEnumerator DeactivateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Chờ 1 giây
        damageTextObject.SetActive(false); // Deactivate damageTextObject
    }

    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        float takeDamage = this.currentHealth - currentHealth;

        this.currentHealth = currentHealth;
        this.maxHealth = maxHealth;

        healthBar.fillAmount = currentHealth / maxHealth;
        UpdateHealthText();

        //if (takeDamage > 0)
        //{
        //    UpdateTextDamage("- " + takeDamage.ToString(), Color.red);
        //}
        //else
        //{
        //    UpdateTextDamage("+ " + Mathf.Abs(takeDamage).ToString(), Color.green);
        //}
    }


    // Cập nhật text hiển thị máu
    public void UpdateHealthText()
    {
        healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }


    // reset tất cả thông tin
    public void ResetHealth()
    {
        maxHealth = 1000f;
        currentHealth = maxHealth;
        healthBar.fillAmount = currentHealth / maxHealth;
        UpdateHealthText();
    }


    private void OnDisable()
    {
        ResetHealth();
    }

}
