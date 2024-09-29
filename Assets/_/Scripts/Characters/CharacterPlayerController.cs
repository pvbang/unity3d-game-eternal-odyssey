using GLTF.Schema;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterPlayerController : MonoBehaviour
{
    private Animator playerAnimator;
    public Transform charactersObject;

    private bool isPressKey = false;

    private string stop = "stop";

    private string walk_up = "walk_up";
    private string walk_down = "walk_down";
    private string walk_left = "walk_left";
    private string walk_right = "walk_right";

    private string run_up = "run_up";



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
        // Di chuyển lên trên
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (isPressKey == false)
            {
                isPressKey = true;
                playerAnimator.SetTrigger(walk_up);
            }
        }

        // Nếu nhấn Shift + UpArrow thì chuyển sang trạng thái chạy
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKey(KeyCode.UpArrow))
        {
            if (isPressKey == false)
            {
                isPressKey = true;
                playerAnimator.SetTrigger(run_up);
            }
        }




        if (
            Input.GetKeyUp(KeyCode.UpArrow) ||
            Input.GetKeyUp(KeyCode.LeftArrow) ||
            Input.GetKeyUp(KeyCode.RightArrow) ||
            Input.GetKeyUp(KeyCode.DownArrow) ||
            Input.GetKeyUp(KeyCode.LeftShift) ||
            Input.GetKeyUp(KeyCode.RightShift)
        ) {
            isPressKey = false;
            playerAnimator.SetTrigger(stop);
        }



        
    }
}
