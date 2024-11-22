using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Image healthBar;
    public Text healthText;

    public GameObject damageTextObject;

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
        // Tạo object mới từ damageTextObject
        GameObject newObject = Instantiate(damageTextObject, damageTextObject.transform.position, damageTextObject.transform.rotation);

        // Đặt newObject làm con của damageTextObject
        newObject.transform.SetParent(damageTextObject.transform.parent);

        // Reset vị trí và rotation tương đối với cha
        newObject.transform.localScale = Vector3.one;

        // Lấy component Text từ object mới
        Text damageText = newObject.GetComponent<Text>();

        // Cập nhật text và màu
        damageText.text = damage;
        damageText.color = color;

        // Hủy object sau 1.2s
        Destroy(newObject, 1.2f);
    }

    private IEnumerator DeactivateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Chờ 1 giây
        damageTextObject.SetActive(false); // Deactivate damageTextObject
    }

    public void UpdateHealth(float currentHealth, float maxHealth, bool isSameEnemy)
    {
        float takeDamage = this.currentHealth - currentHealth;

        this.currentHealth = currentHealth;
        this.maxHealth = maxHealth;

        healthBar.fillAmount = currentHealth / maxHealth;
        UpdateHealthText();

        if (takeDamage == 0 || !isSameEnemy) return;

        else if(takeDamage > 0)
        {
            UpdateTextDamage("- " + takeDamage.ToString(), Color.red);
        }
        else
        {
            UpdateTextDamage("+ " + Mathf.Abs(takeDamage).ToString(), Color.green);
        }
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
