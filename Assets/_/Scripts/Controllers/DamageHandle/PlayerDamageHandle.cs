using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageHandle : MonoBehaviour
{
    CharacterPlayerController characterPlayerController;

    private void Awake()
    {
        characterPlayerController = GetComponent<CharacterPlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ConstantManager.ENEMY_SKILL_TAG))
        {
            TakeDamageValue takeDamageValue = other.GetComponent<TakeDamageValue>();
            if (takeDamageValue != null) TakeDamage(takeDamageValue);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(ConstantManager.ENEMY_SKILL_TAG))
        {
            TakeDamageValue takeDamageValue = collision.gameObject.GetComponent<TakeDamageValue>();
            if (takeDamageValue != null) TakeDamage(takeDamageValue);
        }
    }

    private void TakeDamage(TakeDamageValue takeDamageValue)
    {
        int resistance = 0;

        PlayerDetails playerDetails = PlayerSystemController.instance.playerAtributeUI.GetPlayerDetails();
        if (playerDetails != null)
        {
            Attribute attribute = playerDetails.attribute;
            resistance = attribute.resistance;
        }

        int damage = takeDamageValue.GetDamage() - resistance;
        if (damage < 0)  return;

        bool isCtrlHeld = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
        if (isCtrlHeld)
        {
            characterPlayerController.PlayAnimation("defend_hit");
        } else
        {
            characterPlayerController.PlayAnimation("get_hit");
            PlayerSystemController.instance.TakeDamage(damage);
        }
    }
}
