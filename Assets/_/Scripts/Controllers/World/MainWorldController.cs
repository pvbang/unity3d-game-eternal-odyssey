using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainWorldController : MonoBehaviour
{
    public UIMainWorld UIMainWorld;
    public Content content;

    private void Awake()
    {
        if (UIMainWorld != null) return;
        UIMainWorld = GetComponent<UIMainWorld>();
    }

    private void Start()
    {
        StartCoroutine(ShowListAllWorlds());
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
        world.startAt = System.DateTime.Now.ToString("dd-mm-yyyy.HH:mm:ss:fff");

        StartCoroutine(WorldManager.AddWorld(world));
    }


    // hiển thị danh sách tất cả servers
    IEnumerator ShowListAllWorlds()
    {
        List<World> worlds = new();

        WorldManager.GetAllWorlds(_worlds =>
        {
            worlds = _worlds;
        });


        float timeout = 30f; // Thời gian chờ tối đa 30 giây
        float elapsedTime = 0f;

        while (worlds.Count == 0 && elapsedTime < timeout)
        {
            // Chờ 0.1 giây mỗi lần kiểm tra
            yield return new WaitForSeconds(0.1f);

            // Cập nhật thời gian đã trôi qua
            elapsedTime += 0.1f;
        }

        if (worlds.Count == 0)
        {
            Debug.LogWarning("| Timeout: ShowListAllWorlds.");
        }
        else
        {
            List<Transform> contentList = new List<Transform>();
            foreach (World world in worlds)
            {
                Transform itemTransform = Instantiate(content.GetItem());
                itemTransform.GetComponent<UIItemWorld>().SetData(world.name, world.status.ToString(), world.code, world.startAt, world.endAt);

                contentList.Add(itemTransform);
            }

            content.DestroyContents();
            content.AddContent(contentList);
        }
    }
}
