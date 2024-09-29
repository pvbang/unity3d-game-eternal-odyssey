using GLTF.Schema;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterPlayerController : MonoBehaviour
{
    private Animator playerAnimator;
    public Transform charactersObject;

    private string idle = "idle";
    private string walk_up = "walk_up";
    private string walk_down = "walk_down";
    private string walk_left = "walk_left";
    private string walk_right = "walk_right";



    private void Start()
    {
        if (charactersObject == null)
        {
            charactersObject = transform.Find("Characters");
        }
        
        CharacterObjectController characterObjectController = charactersObject.GetComponent<CharacterObjectController>();
        if (characterObjectController != null)
        {
            playerAnimator = characterObjectController.GetActiveCharacterAnimator();
        }
    }


    void Update()
    {
        GetKeyInput();
    } 


    // Hàm lấy input từ bàn phím
    public void GetKeyInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Debug.Log("space key was pressed");
        }

        // Di chuyển sang trái
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Debug.Log("LeftArrow");
            playerAnimator.SetBool(walk_left, true);
        }
        else
        {
            playerAnimator.SetBool(walk_left, false);
        }

        // Di chuyển lên trên
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Debug.Log("UpArrow");
            playerAnimator.SetTrigger(walk_up);
        }
        else
        {
            playerAnimator.ResetTrigger(walk_up);
            playerAnimator.SetBool(walk_up, false);
        }



        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("RightArrow");
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Debug.Log("DownArrow");
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("A");
        }
    }
}
