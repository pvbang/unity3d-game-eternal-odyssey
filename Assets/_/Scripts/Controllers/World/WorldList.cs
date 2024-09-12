using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldList : MonoBehaviour
{
    [Header("Save To Database")]
    public bool save = false;

    [Header("Item Information")]
    public World world;

    private void Start()
    {
        if (world == null)
        {
            return;
        }

        // nếu saveToDatabase = true thì lưu item này vào database
        if (save)
        {
            // nếu item này chưa nhập vào id thì sẽ lấy id của gameObject, không thì sẽ lấy id mình nhập vào của item
            if (world.worldId == "")
            {
                world.worldId = gameObject.name;
            }

            // thêm item vào database
            StartCoroutine(WorldManager.AddWorld(world));
        }
    }
}
