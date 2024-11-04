using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSystemController : MonoBehaviour
{
    public static PlayerSystemController instance;

    [SerializeField]
    private PlayerAtributeUI playerAtributeUI;

    private void Awake()
    {
        instance = this;
    }

    //new Dictionary<string, object>
    //{
    //    { "experience", 10f },
    //    { "attribute/health/current", 100f }
    //}
    private void UpdateValuePlayer(Dictionary<string, object> changedValue)
    {
        // Lấy worldId từ PlayerPrefs
        string worldId = PlayerPrefs.GetString(ConstantManager.FIREBASE_WORLDS_ID);
        if (string.IsNullOrEmpty(worldId)) return;

        // Lấy userId từ FirebaseManager
        string userId = FirebaseManager.instance.GetUserId();
        if (string.IsNullOrEmpty(userId)) return;

        // Gọi hàm UpdatePlayerDetails với các thông tin cập nhật
        StartCoroutine(WorldManager.UpdateFieldsPlayerDetails(
            worldId,
            userId,
            changedValue
        ));
    }

    public void UpdatePlayer(Dictionary<string, object> changedValue)
    {
        UpdateValuePlayer(changedValue);
    }

    // Nhận sát thương
    public void TakeDamage(float damage)
    {
        PlayerDetails playerDetails = playerAtributeUI.GetPlayerDetails();
        if (playerDetails == null) return;

        Attribute attribute = playerDetails.attribute;

        float currentHealth = attribute.health.current;
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        Dictionary<string, object> changedValue = new Dictionary<string, object>
        {
            { "attribute/health/current", currentHealth }
        };

        UpdateValuePlayer(changedValue);
    }


    // Hồi máu
    public void Heal(float heal)
    {
        PlayerDetails playerDetails = playerAtributeUI.GetPlayerDetails();
        if (playerDetails == null) return;

        Attribute attribute = playerDetails.attribute;

        float currentHealth = attribute.health.current;
        currentHealth += heal;
        if (currentHealth > attribute.health.max) currentHealth = attribute.health.max;

        Dictionary<string, object> changedValue = new Dictionary<string, object>
        {
            { "attribute/health/current", currentHealth }
        };

        UpdateValuePlayer(changedValue);
    }

    // Hàm buff tăng sát thương trong khoảng thời gian nhất định
    public void BuffDamage(float buff, float duration)
    {
        StartCoroutine(BuffDamageCoroutine(buff, duration));
    }

    private IEnumerator BuffDamageCoroutine(float buff, float duration)
    {
        PlayerDetails playerDetails = playerAtributeUI.GetPlayerDetails();
        if (playerDetails == null) yield break;

        Attribute attribute = playerDetails.attribute;

        float currentDame = attribute.Damage.from;
        currentDame *= (buff / 100);

        float maxDame = attribute.Damage.to;
        maxDame *= (buff / 100);

        Dictionary<string, object> changedValue = new Dictionary<string, object>
        {
            { "attribute/damage/from", currentDame },
            { "attribute/damage/to", maxDame }
        };

        UpdateValuePlayer(changedValue);

        yield return new WaitForSeconds(duration);

        Dictionary<string, object> changedValue2 = new Dictionary<string, object>
        {
            { "attribute/damage/from", attribute.Damage.from },
            { "attribute/damage/to", attribute.Damage.to }
        };

        UpdateValuePlayer(changedValue2);
    }


    // Hàm buff tăng máu tạm thời trong một khoảng thời gian
    public void BuffHealth(float buffHealth, float duration)
    {
        StartCoroutine(BuffHealthCoroutine(buffHealth, duration));
    }

    private IEnumerator BuffHealthCoroutine(float buffHealth, float duration)
    {
        PlayerDetails playerDetails = playerAtributeUI.GetPlayerDetails();
        if (playerDetails == null) yield break;
        Attribute attribute = playerDetails.attribute;

        float originalMaxHealth = attribute.health.max;
        attribute.health.max += buffHealth;
        attribute.health.current += buffHealth;

        Dictionary<string, object> changedValue = new Dictionary<string, object>
        {
            { "attribute/health/max", attribute.health.max },
            { "attribute/health/current", attribute.health.current }
        };

        UpdateValuePlayer(changedValue);

        yield return new WaitForSeconds(duration);

        Dictionary<string, object> changedValue2 = new Dictionary<string, object>
        {
            { "attribute/health/max", originalMaxHealth },
            { "attribute/health/current", attribute.health.current }
        };

        UpdateValuePlayer(changedValue2);
    }


    // Hàm buff tăng máu vĩnh viễn
    public void BuffMaxHealth(float buffHealth)
    {
        PlayerDetails playerDetails = playerAtributeUI.GetPlayerDetails();
        if (playerDetails == null) return;

        Attribute attribute = playerDetails.attribute;

        attribute.health.max += buffHealth;
        attribute.health.current += buffHealth;

        Dictionary<string, object> changedValue = new Dictionary<string, object>
        {
            { "attribute/health/max", attribute.health.max },
            { "attribute/health/current", attribute.health.current }
        };

        UpdateValuePlayer(changedValue);
    }

    // Hàm debuff giảm máu vĩnh viễn
    public void DebuffMaxHealth(float debuffHealth)
    {
        PlayerDetails playerDetails = playerAtributeUI.GetPlayerDetails();
        if (playerDetails == null) return;

        Attribute attribute = playerDetails.attribute;

        attribute.health.max -= debuffHealth;
        if (attribute.health.current > attribute.health.max)
        {
            attribute.health.current = attribute.health.max;
        }

        Dictionary<string, object> changedValue = new Dictionary<string, object>
        {
            { "attribute/health/max", attribute.health.max },
            { "attribute/health/current", attribute.health.current }
        };

        UpdateValuePlayer(changedValue);
    }

    // Hàm debuff giảm máu tạm thời trong một khoảng thời gian
    public void DebuffHealth(float debuffHealth, float duration)
    {
        StartCoroutine(DebuffHealthCoroutine(debuffHealth, duration));
    }

    private IEnumerator DebuffHealthCoroutine(float debuffHealth, float duration)
    {
        PlayerDetails playerDetails = playerAtributeUI.GetPlayerDetails();
        if (playerDetails == null) yield break;

        Attribute attribute = playerDetails.attribute;

        float originalMaxHealth = attribute.health.max;
        attribute.health.max -= debuffHealth;
        if (attribute.health.current > attribute.health.max)
        {
            attribute.health.current = attribute.health.max;
        }

        Dictionary<string, object> changedValue = new Dictionary<string, object>
        {
            { "attribute/health/max", attribute.health.max },
            { "attribute/health/current", attribute.health.current }
        };

        UpdateValuePlayer(changedValue);

        yield return new WaitForSeconds(duration);

        Dictionary<string, object> changedValue2 = new Dictionary<string, object>
        {
            { "attribute/health/max", originalMaxHealth },
            { "attribute/health/current", attribute.health.current }
        };

        UpdateValuePlayer(changedValue2);
    }

    // Buff tăng chống chịu trong khoảng thời gian nhất định
    public void BuffResistance(float buff, float duration)
    {
        StartCoroutine(BuffResistanceCoroutine(buff, duration));
    }

    private IEnumerator BuffResistanceCoroutine(float buff, float duration)
    {
        PlayerDetails playerDetails = playerAtributeUI.GetPlayerDetails();
        if (playerDetails == null) yield break;

        Attribute attribute = playerDetails.attribute;

        float currentResistance = attribute.resistance;
        currentResistance *= (buff / 100);

        Dictionary<string, object> changedValue = new Dictionary<string, object>
        {
            { "attribute/resistance", currentResistance }
        };

        UpdateValuePlayer(changedValue);

        yield return new WaitForSeconds(duration);

        Dictionary<string, object> changedValue2 = new Dictionary<string, object>
        {
            { "attribute/resistance", attribute.resistance }
        };

        UpdateValuePlayer(changedValue2);
    }


    // Buff tăng tốc độ di chuyển trong khoảng thời gian nhất định
    public void BuffSpeed(float buff, float duration)
    {
        StartCoroutine(BuffSpeedCoroutine(buff, duration));
    }

    private IEnumerator BuffSpeedCoroutine(float buff, float duration)
    {
        PlayerDetails playerDetails = playerAtributeUI.GetPlayerDetails();
        if (playerDetails == null) yield break;

        Attribute attribute = playerDetails.attribute;

        float currentSpeed = attribute.runSpeed;
        currentSpeed *= (buff / 100);

        Dictionary<string, object> changedValue = new Dictionary<string, object>
        {
            { "attribute/speed", currentSpeed }
        };

        UpdateValuePlayer(changedValue);

        yield return new WaitForSeconds(duration);

        Dictionary<string, object> changedValue2 = new Dictionary<string, object>
        {
            { "attribute/speed", attribute.runSpeed }
        };

        UpdateValuePlayer(changedValue2);
    }

    // Buff tăng tốc độ tấn công trong khoảng thời gian nhất định
    public void BuffAttackSpeed(float buff, float duration)
    {
        StartCoroutine(BuffAttackSpeedCoroutine(buff, duration));
    }

    private IEnumerator BuffAttackSpeedCoroutine(float buff, float duration)
    {
        PlayerDetails playerDetails = playerAtributeUI.GetPlayerDetails();
        if (playerDetails == null) yield break;

        Attribute attribute = playerDetails.attribute;

        float currentAttackSpeed = attribute.attackSpeed;
        currentAttackSpeed *= (buff / 100);

        Dictionary<string, object> changedValue = new Dictionary<string, object>
        {
            { "attribute/attackSpeed", currentAttackSpeed }
        };

        UpdateValuePlayer(changedValue);

        yield return new WaitForSeconds(duration);

        Dictionary<string, object> changedValue2 = new Dictionary<string, object>
        {
            { "attribute/attackSpeed", attribute.attackSpeed }
        };

        UpdateValuePlayer(changedValue2);
    }


    // Buff tăng critical chance trong khoảng thời gian nhất định
    public void BuffCriticalChance(float buff, float duration)
    {
        StartCoroutine(BuffCriticalChanceCoroutine(buff, duration));
    }

    private IEnumerator BuffCriticalChanceCoroutine(float buff, float duration)
    {
        PlayerDetails playerDetails = playerAtributeUI.GetPlayerDetails();
        if (playerDetails == null) yield break;

        Attribute attribute = playerDetails.attribute;

        float currentCriticalChance = attribute.crit;
        currentCriticalChance *= (buff / 100);

        Dictionary<string, object> changedValue = new Dictionary<string, object>
        {
            { "attribute/crit", currentCriticalChance }
        };

        UpdateValuePlayer(changedValue);

        yield return new WaitForSeconds(duration);

        Dictionary<string, object> changedValue2 = new Dictionary<string, object>
        {
            { "attribute/crit", attribute.crit }
        };

        UpdateValuePlayer(changedValue2);
    }


    // Buff tăng intelligence trong khoảng thời gian nhất định
    public void BuffIntelligence(float buff, float duration)
    {
        StartCoroutine(BuffIntelligenceCoroutine(buff, duration));
    }

    private IEnumerator BuffIntelligenceCoroutine(float buff, float duration)
    {
        PlayerDetails playerDetails = playerAtributeUI.GetPlayerDetails();
        if (playerDetails == null) yield break;

        Attribute attribute = playerDetails.attribute;

        float currentIntelligence = attribute.intelligence;
        currentIntelligence *= (buff / 100);

        Dictionary<string, object> changedValue = new Dictionary<string, object>
        {
            { "attribute/intelligence", currentIntelligence }
        };

        UpdateValuePlayer(changedValue);

        yield return new WaitForSeconds(duration);

        Dictionary<string, object> changedValue2 = new Dictionary<string, object>
        {
            { "attribute/intelligence", attribute.intelligence }
        };

        UpdateValuePlayer(changedValue2);
    }
}
