using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSystemController : MonoBehaviour
{
    public static PlayerSystemController instance;

    [SerializeField]
    public PlayerAtributeUI playerAtributeUI;

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
    public void TakeDamage(int damage)
    {
        PlayerDetails playerDetails = playerAtributeUI.GetPlayerDetails();
        if (playerDetails == null) return;

        Attribute attribute = playerDetails.attribute;

        int currentHealth = attribute.health.current;
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        Dictionary<string, object> changedValue = new Dictionary<string, object>
        {
            { "attribute/health/current", currentHealth }
        };

        UpdateValuePlayer(changedValue);
    }


    // Hồi máu
    public bool Heal(int heal)
    {
        PlayerDetails playerDetails = playerAtributeUI.GetPlayerDetails();
        if (playerDetails == null) return false;

        Attribute attribute = playerDetails.attribute;

        int currentHealth = attribute.health.current;
        if (currentHealth > attribute.health.max)
        {
            Debug.Log("Máu đã đầy, không cần hồi máu thêm.");
            return false; // Trả về false để biểu thị rằng không cần hồi máu
        }

        currentHealth += heal;
        if (currentHealth > attribute.health.max) currentHealth = attribute.health.max;

        Dictionary<string, object> changedValue = new Dictionary<string, object>
        {
            { "attribute/health/current", currentHealth }
        };

        UpdateValuePlayer(changedValue);
        return true; // Trả về true để biểu thị rằng đã hồi máu thành công
    }

    // Hàm buff tăng sát thương trong khoảng thời gian nhất định
    public void BuffDamage(int buff, float duration)
    {
        StartCoroutine(BuffDamageCoroutine(buff, duration));
    }

    private IEnumerator BuffDamageCoroutine(int buff, float duration)
    {
        PlayerDetails playerDetails = playerAtributeUI.GetPlayerDetails();
        if (playerDetails == null) yield break;

        Attribute attribute = playerDetails.attribute;

        int currentDame = attribute.Damage.from;
        currentDame *= (buff / 100);

        int maxDame = attribute.Damage.to;
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
    public void BuffHealth(int buffHealth, float duration)
    {
        StartCoroutine(BuffHealthCoroutine(buffHealth, duration));
    }

    private IEnumerator BuffHealthCoroutine(int buffHealth, float duration)
    {
        PlayerDetails playerDetails = playerAtributeUI.GetPlayerDetails();
        if (playerDetails == null) yield break;
        Attribute attribute = playerDetails.attribute;

        int originalMaxHealth = attribute.health.max;
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
    public void BuffMaxHealth(int buffHealth)
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
    public void DebuffMaxHealth(int debuffHealth)
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
    public void DebuffHealth(int debuffHealth, float duration)
    {
        StartCoroutine(DebuffHealthCoroutine(debuffHealth, duration));
    }

    private IEnumerator DebuffHealthCoroutine(int debuffHealth, float duration)
    {
        PlayerDetails playerDetails = playerAtributeUI.GetPlayerDetails();
        if (playerDetails == null) yield break;

        Attribute attribute = playerDetails.attribute;

        int originalMaxHealth = attribute.health.max;
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
    public void BuffResistance(int buff, float duration)
    {
        StartCoroutine(BuffResistanceCoroutine(buff, duration));
    }

    private IEnumerator BuffResistanceCoroutine(int buff, float duration)
    {
        PlayerDetails playerDetails = playerAtributeUI.GetPlayerDetails();
        if (playerDetails == null) yield break;

        Attribute attribute = playerDetails.attribute;

        int currentResistance = attribute.resistance;
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
        // làm tròn đến 2 chữ số thập phân
        currentSpeed = Mathf.Round(currentSpeed * 100f) / 100f;

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
        // làm tròn đến 2 chữ số thập phân
        currentAttackSpeed = Mathf.Round(currentAttackSpeed * 100f) / 100f;

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
        // làm tròn đến 2 chữ số thập phân
        currentCriticalChance = Mathf.Round(currentCriticalChance * 100f) / 100f;

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
        // làm tròn đến 2 chữ số thập phân
        currentIntelligence = Mathf.Round(currentIntelligence * 100f) / 100f;

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

    ///////////////////////////////////////////////////////////////////////////////////////
    // LOCATION

    private void UpdateValueLocationPlayer(Dictionary<string, object> changedValue)
    {
        // Lấy worldId từ PlayerPrefs
        string worldId = PlayerPrefs.GetString(ConstantManager.FIREBASE_WORLDS_ID);
        if (string.IsNullOrEmpty(worldId)) return;

        // Lấy userId từ FirebaseManager
        string userId = FirebaseManager.instance.GetUserId();
        if (string.IsNullOrEmpty(userId)) return;

        // Gọi hàm UpdatePlayerDetails với các thông tin cập nhật
        StartCoroutine(WorldManager.UpdateFieldsLocation(
            worldId,
            userId,
            changedValue
        ));
    }

    // Hàm cập nhật vị trí của player
    public void UpdateLocation(Vector3 position, Vector3 rotation)
    {
        Dictionary<string, object> changedValue = new Dictionary<string, object>
        {
            { "position/x", position.x },
            { "position/y", position.y },
            { "position/z", position.z },
            { "rotation/x", rotation.x },
            { "rotation/y", rotation.y },
            { "rotation/z", rotation.z }
        };

        UpdateValueLocationPlayer(changedValue);
    }

    // Hàm cập nhật animation của player
    public void UpdateCharacterState(CharacterState characterState)
    {
        Dictionary<string, object> changedValue = new Dictionary<string, object>
        {
            { "characterState", (int)characterState }
        };

        UpdateValueLocationPlayer(changedValue);
    }
}
