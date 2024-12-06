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
<<<<<<< HEAD
        if (collision.gameObject.CompareTag(ConstantManager.ENEMY_SKILL_TAG))
        {
=======
        if (collision.gameObject.CompareTag(ConstantManager.SKILL_TAG))
        {
            bool isCtrlHeld = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);

            if (isCtrlHeld)
            {
                Debug.Log("T c� khi�nnnn!!!");
                return;
            }

            characterPlayerController.SetCharacterState(CharacterState.GetHit);
>>>>>>> 4db4d9d19cf2dad80f9858d824700e79419ea821
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
        Debug.Log($"Player took {damage} damage!");
        if (damage < 0)  return;

<<<<<<< HEAD
        bool isCtrlHeld = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
        if (isCtrlHeld)
        {
            characterPlayerController.PlayAnimation("defend_hit");
        } else
        {
            characterPlayerController.PlayAnimation("get_hit");
            PlayerSystemController.instance.TakeDamage(damage);
        }
=======
        PlayerSystemController.instance.TakeDamage(damage);
>>>>>>> 4db4d9d19cf2dad80f9858d824700e79419ea821
    }
}
