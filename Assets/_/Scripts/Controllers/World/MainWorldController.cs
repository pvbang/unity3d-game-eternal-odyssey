using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainWorldController : MonoBehaviour
{
    public UIMainWorld UIMainWorld;

    private void Start()
    {
        UIMainWorld = GetComponent<UIMainWorld>();
    }

    public void CreateWorld()
    {
        World world = new World();
        world.worldId = RandomManager.GenerateRandomID();
        world.name = UIMainWorld.GetNameWorld();
        world.code = UIMainWorld.GetCodeWorld();
        world.questWorld = new List<QuestWorld>();
        world.houseWorld = new List<HouseWorld>();
        world.playerWorld = new List<PlayerWorld>();
        world.buildingWorld = new List<BuildingWorld>();
        world.status = Status.NotStarted;
        world.startAt = System.DateTime.Now;

        StartCoroutine(WorldManager.AddWorld(world));
    }

}
