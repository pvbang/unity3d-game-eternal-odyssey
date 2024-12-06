using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HouseBase : MonoBehaviour
{
    [Header("Save To Database")]
    public bool save = false;

    [Header("House Information")]
    public House house;

    private List<MaterialsUpdateBase> materialsUpdateBase;

    private void Awake()
    {
        // Lấy tất cả các component MaterialsUpdateBase từ cùng GameObject và chuyển thành List
        materialsUpdateBase = gameObject.GetComponents<MaterialsUpdateBase>().ToList();
    }

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

            if (materialsUpdateBase != null && materialsUpdateBase.Count > 0)
            {
                // Lấy materialsUpdate từ tất cả các MaterialsUpdateBase
                List<MaterialsUpdate> allMaterialsUpdates = new List<MaterialsUpdate>();

                foreach (var materialBase in materialsUpdateBase)
                {
                    allMaterialsUpdates.AddRange(materialBase.GetMaterialsUpdate());
                }

                // Gán lần lượt MaterialsUpdate vào houseFeatures như đã thực hiện trước đó
                int maxFeatures = Mathf.Min(house.houseFeatures.Count, allMaterialsUpdates.Count);
                for (int i = 0; i < maxFeatures; i++)
                {
                    house.houseFeatures[i].materialUpdates = new List<MaterialsUpdate> { allMaterialsUpdates[i] };
                }
            }

            // thêm house vào database
            StartCoroutine(HouseManager.Add(house));
        }
    }
}
