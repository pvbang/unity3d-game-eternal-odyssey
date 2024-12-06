using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationSkillUpdate : MonoBehaviour
{
    // Đối tượng cần xoay
    public Camera mainCamera;


    void FixedUpdate()
    {
        float _x = mainCamera.transform.rotation.eulerAngles.x;

        if (_x < 270) _x = 360;
        transform.rotation = Quaternion.Euler(_x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}
