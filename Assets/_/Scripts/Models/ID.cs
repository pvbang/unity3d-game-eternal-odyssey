using UnityEngine;

public class ID : MonoBehaviour
{
    public string id = "";

    public string GetID()
    {
        if (id == "")
        {
            id = gameObject.name;

            // nếu có (Clone) thì xóa đi
            if (id.Contains("(Clone)"))
            {
                id = id.Replace("(Clone)", "");
            }
        }

        return id;
    }
}
