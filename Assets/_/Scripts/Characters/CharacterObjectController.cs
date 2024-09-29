using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterObjectController : MonoBehaviour
{
    public GameObject playerObject = null;
    public Animator playerAnimator;

    private void Awake()
    {
        // Duyệt qua các con của đối tượng hiện tại và tìm ra con đang active
        GameObject activeChild = GetActiveChildObject(this.gameObject);

        if (activeChild != null)
        {
            playerObject = activeChild;
            playerAnimator = playerObject.GetComponent<Animator>();
        }
    }


    // Hàm để tìm Object con đang active
    GameObject GetActiveChildObject(GameObject parent)
    {
        foreach (Transform child in parent.transform)
        {
            if (child.gameObject.activeSelf) // Kiểm tra nếu Object con đang active
            {
                return child.gameObject;
            }
        }
        return null; // Nếu không có Object con nào active
    }

    // Hàm để lấy Object con đang active
    public GameObject GetActiveCharacter()
    {
        return playerObject;
    }

    // Hàm để lấy Animator của Object con đang active
    public Animator GetActiveCharacterAnimator()
    {
        return playerAnimator;
    }
}
