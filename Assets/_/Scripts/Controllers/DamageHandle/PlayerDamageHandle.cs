using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageHandle : MonoBehaviour
{
    public int minDamage = 0;
    public int maxDamage = 0;

    CharacterPlayerController characterPlayerController;

    private void Awake()
    {
        characterPlayerController = GetComponent<CharacterPlayerController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ConstantManager.SKILL_TAG))
        {
            bool isCtrlHeld = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);

            if (isCtrlHeld)
            {
                Debug.Log("T có khiênnnn!!!");
                return;
            }

            Debug.Log("PlayerDamageHandle");
            int damage = Random.Range(minDamage, maxDamage);
            Debug.Log("Damage: " + damage);
            characterPlayerController.SetCharacterState(CharacterState.GetHit);

            if (damage <= 0)
            {
                return;
            }

            PlayerSystemController.instance.TakeDamage(damage);
        }
    }


    public void SetDamage(int min, int max)
    {
        minDamage = min;
        maxDamage = max;
    }
}
