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
        if (other.CompareTag(ConstantManager.SKILL_TAG))
        {
            TakeDamageValue takeDamageValue = other.GetComponent<TakeDamageValue>();
            if (takeDamageValue != null) TakeDamage(takeDamageValue);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(ConstantManager.SKILL_TAG))
        {
            bool isCtrlHeld = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);

            if (isCtrlHeld)
            {
                Debug.Log("T có khiênnnn!!!");
                return;
            }

            characterPlayerController.SetCharacterState(CharacterState.GetHit);
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

        PlayerSystemController.instance.TakeDamage(damage);
    }
}
