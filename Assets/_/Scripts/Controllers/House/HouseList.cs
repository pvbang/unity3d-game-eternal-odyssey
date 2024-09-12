using UnityEngine;

public class HouseList : MonoBehaviour
{
    [Header("Save To Database")]
    public bool save = false;

    [Header("Item Information")]
    public House house;

    private void Start()
    {
        if (house == null)
        {
            return;
        }

        // nếu saveToDatabase = true thì lưu item này vào database
        if (save)
        {
            // nếu house này chưa nhập vào id thì sẽ lấy id của gameObject, không thì sẽ lấy id mình nhập vào của item
            if (house.houseId == "")
            {
                house.houseId = gameObject.name;
            }

            // thêm house vào database
            StartCoroutine(HouseManager.AddHouse(house));
        }
    }
}
