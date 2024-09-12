using UnityEngine;

[System.Serializable]
public class WeaponList : MonoBehaviour
{
    [Header("Save To Database")]
    public bool save = false;

    [Header("Item Information")]
    public Weapon weapon;

    private void Start()
    {
        if (weapon == null)
        {
            return;
        }

        // nếu saveToDatabase = true thì lưu item này vào database
        if (save)
        {
            // nếu item này chưa nhập vào id thì sẽ lấy id của gameObject, không thì sẽ lấy id mình nhập vào của item
            if (weapon.weaponId == "")
            {
                weapon.weaponId = gameObject.name;
            }

            // thêm item vào database
            StartCoroutine(WeaponManager.AddWeapon(weapon));
        }
    }
}
