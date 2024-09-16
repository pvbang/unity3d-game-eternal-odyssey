using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnim : MonoBehaviour
{
    public Animator animator;
    public string triggerName;

    public bool is0nEnable = false;
    public bool is0nDisable = false;

    public void PlayAnimation()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        animator.SetTrigger(triggerName);
    }

    private void OnEnable()
    {
        if (is0nEnable)
            PlayAnimation();
    }

    private void OnDisable()
    {
        if (is0nDisable)
            PlayAnimation();
    }
}