using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAtributeUI : MonoBehaviour
{
    public Text txtLevel;
    public Text txtName;
    public HealthBarUI healthBarUI;
    private PlayerDetails playerDetails;

    private DatabaseReference referenceGames;

    private void OnEnable()
    {
        if (!FirebaseManager.instance.IsLogin()) return;

        string worldId = PlayerPrefs.GetString(ConstantManager.FIREBASE_WORLDS_ID);
        if (string.IsNullOrEmpty(worldId)) return;

        referenceGames = FirebaseManager.instance.databaseReference
            .Child(ConstantManager.FIREBASE_WORLDS)
            .Child(worldId)
            .Child(ConstantManager.FIREBASE_WORLDS_PLAYERS)
            .Child(FirebaseManager.instance.GetUserId())
            .Child(ConstantManager.FIREBASE_WORLDS_PLAYER_DETAILS);

        // Đăng ký sự kiện
        referenceGames.ValueChanged += HandleValueChangedPlayerDetails;
    }

    private void OnDisable()
    {
        // Hủy đăng ký sự kiện để tránh việc sự kiện còn tồn tại khi chuyển scene
        if (referenceGames != null)
        {
            referenceGames.ValueChanged -= HandleValueChangedPlayerDetails;
        }
    }

    void HandleValueChangedPlayerDetails(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError($"Database Error: {args.DatabaseError.Message}");
            return;
        }

        if (!args.Snapshot.Exists)
        {
            Debug.LogWarning("Snapshot is empty, no data to display.");
            return;
        }

        StartCoroutine(UpdateUI(args.Snapshot));
    }

    private IEnumerator UpdateUI(DataSnapshot data)
    {
        if (!data.Exists) yield break;
        
        PlayerDetails playerDetails = JsonUtility.FromJson<PlayerDetails>(data.GetRawJsonValue());
        if (playerDetails == this.playerDetails) yield break;

        this.playerDetails = playerDetails;

        // lấy level
        string playerLevel = playerDetails.level.ToString();
        Match match = Regex.Match(playerLevel, @"\d+$");
        string levelNumber = match.Value;
        txtLevel.text = levelNumber;

        // lấy tên
        txtName.text = playerDetails.name;

        // lấy health
        Attribute attribute = playerDetails.attribute;
        Stats health = attribute.health;
        healthBarUI.UpdateHealth(health.current, health.max);

        // lấy experience
        Stats experience = playerDetails.experience;
        if (experience.current >= experience.max)
        {
            if (playerDetails.level == Level.Level_30)
            {
                yield break;
            }

            // Chuyển enum sang số nguyên và cộng thêm 1, sau đó kiểm tra giới hạn của enum
            int numberLevel = int.Parse(levelNumber);

            int nextLevel = numberLevel + 1;
            float nextMaxExperience = experience.max * 1.1f;
            float maxHealth = health.max * 1.1f;
            float fromDamage = attribute.damage.from * 1.1f;
            float maxDamage = attribute.damage.to * 1.1f;
            float resistance = attribute.resistance * 1.1f;
            float attackSpeed = attribute.attackSpeed * 1.1f;
            float runSpeed = attribute.runSpeed * 1.1f;
            float intelligence = attribute.intelligence * 1.1f;
            float crit = attribute.crit * 1.1f;

            Attribute newAttribute = new Attribute(new Stats(maxHealth, maxHealth), new Rate(fromDamage, maxDamage), resistance, attackSpeed, runSpeed, intelligence, crit);
            UpdateLevel(nextLevel, nextMaxExperience, newAttribute);
        }
    }

    public PlayerDetails GetPlayerDetails()
    {
        return this.playerDetails;
    }

    public void UpdateLevel(int nextLevel, float nextMaxExperience, Attribute newAttribute)
    {
        Dictionary<string, object> changedValue = new Dictionary<string, object>
        {
            { "level", nextLevel },
            { "experience/current", 0f },
            { "experience/max", nextMaxExperience },
            //{ "attribute", newAttribute }
        };

        PlayerSystemController.instance.UpdatePlayer(changedValue);
    }

}
