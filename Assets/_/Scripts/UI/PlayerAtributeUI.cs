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

    private void Start()
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

        int numberLevel = int.Parse(levelNumber);
        PlayerPrefs.SetInt(ConstantManager.PLAYER_LEVEL, numberLevel);

        // lấy tên
        txtName.text = playerDetails.name;

        // lấy health
        Attribute attribute = playerDetails.attribute;
        Stats health = attribute.health;
        healthBarUI.UpdateHealth(health.current, health.max, true);

        // lấy experience
        Stats experience = playerDetails.experience;
        if (experience.current >= experience.max)
        {
            if (playerDetails.level == Level.Level_30)
            {
                yield break;
            }

            float plus = 1.1f;

            int nextLevel = numberLevel + 1;
            int nextMaxExperience = (int)(experience.max * plus);
            int maxHealth = (int)(health.max * plus);
            int fromDamage = (int)(attribute.damage.from * plus);
            int maxDamage = (int)(attribute.damage.to * plus);
            int resistance = (int)(attribute.resistance * plus);

            float attackSpeed = attribute.attackSpeed * plus;
            attackSpeed = (float)Math.Round(attackSpeed, 2);

            float runSpeed = attribute.runSpeed * plus;
            runSpeed = (float)Math.Round(runSpeed, 2);

            float intelligence = attribute.intelligence * plus;
            intelligence = (float)Math.Round(intelligence, 2);

            float crit = attribute.crit * plus;
            crit = (float)Math.Round(crit, 2);

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
            { "attribute/health/current", newAttribute.health.current },
            { "attribute/health/max", newAttribute.health.max },
            { "attribute/damage/from", newAttribute.damage.from },
            { "attribute/damage/to", newAttribute.damage.to },
            { "attribute/resistance", newAttribute.resistance },
            { "attribute/attackSpeed", newAttribute.attackSpeed },
            { "attribute/runSpeed", newAttribute.runSpeed },
            { "attribute/intelligence", newAttribute.intelligence },
            { "attribute/crit", newAttribute.crit }
        };

        PlayerSystemController.instance.UpdatePlayer(changedValue);
    }

}
