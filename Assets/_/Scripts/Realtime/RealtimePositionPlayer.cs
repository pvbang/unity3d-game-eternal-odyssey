using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealtimePositionPlayer : MonoBehaviour
{
    public Transform realtimeObject;

    // Lưu vị trí và góc quay cũ
    private Vector3 previousPosition;
    private Vector3 previousRotation;

    private void Start()
    {
        if (realtimeObject == null)
        {
            // Nếu không có realtimeObject, dùng transform của đối tượng này
            realtimeObject = this.transform;
        }

        // Khởi tạo giá trị ban đầu của vị trí và rotation
        previousPosition = realtimeObject.localPosition;
        previousRotation = realtimeObject.localRotation.eulerAngles;
    }

    private void Update()
    {
        // Tạo Vector3 cho position và rotation (sử dụng localPosition và localRotation)
        Vector3 position = realtimeObject.localPosition;
        Vector3 rotation = realtimeObject.localRotation.eulerAngles;

        // Kiểm tra xem vị trí hoặc góc quay có thay đổi không
        if (position != previousPosition || rotation != previousRotation)
        {
            // Cập nhật vị trí và rotation cho PlayerSystemController
            PlayerSystemController.instance.UpdateLocation(position, rotation);

            // Cập nhật lại vị trí và rotation cũ
            previousPosition = position;
            previousRotation = rotation;
        }
    }
}
