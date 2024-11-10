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
            int damage = Random.Range(minDamage, maxDamage);
            PlayerSystemController.instance.TakeDamage(damage);
            Debug.Log("PlayerDamageHandle");
            //characterPlayerController.SetCharacterState(CharacterState.GetHit);
        }
    }


    public void SetDamage(int min, int max)
    {
        minDamage = min;
        maxDamage = max;
    }
}
