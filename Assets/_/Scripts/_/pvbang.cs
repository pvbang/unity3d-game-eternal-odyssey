using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pvbang : MonoBehaviour
{
    void Start()
    {
        World world = new World();

        world.worldId = RandomManager.GenerateRandomID();
        world.name = "Phòng Test Bằng";
        world.code = "123456";

        StartCoroutine(WorldManager.AddWorld(world));

    }

}
