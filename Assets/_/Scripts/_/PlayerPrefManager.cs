using UnityEngine;

public class PlayerPrefManager : MonoBehaviour
{
    // Enum để chọn kiểu dữ liệu lưu trữ
    public enum DataType
    {
        Int,
        Float,
        String
    }

    public string keyName;       // Tên khóa lưu trữ
    public DataType dataType;    // Loại dữ liệu

    [Header("Data")]
    public string stringValue = "";   // Giá trị kiểu string
    public int intValue = 0;         // Giá trị kiểu int
    public float floatValue = 0f;     // Giá trị kiểu float

    // Hàm lưu dữ liệu dựa trên kiểu dữ liệu đã chọn
    public void Save()
    {
        switch (dataType)
        {
            case DataType.Int:
                PlayerPrefs.SetInt(keyName, intValue);
                break;
            case DataType.Float:
                PlayerPrefs.SetFloat(keyName, floatValue);
                break;
            case DataType.String:
                PlayerPrefs.SetString(keyName, stringValue);
                break;
        }

        PlayerPrefs.Save(); // Lưu thay đổi xuống đĩa
        Debug.Log($"Đã lưu: {keyName} với kiểu {dataType}");
        Debug.Log($"Giá trị: {stringValue} - {intValue} - {floatValue}");
    }

    // Hàm tải dữ liệu dựa trên kiểu dữ liệu đã chọn
    public void Load()
    {
        switch (dataType)
        {
            case DataType.Int:
                intValue = PlayerPrefs.GetInt(keyName, 0); // Giá trị mặc định là 0
                Debug.Log($"Tải {keyName}: {intValue} (Int)");
                break;
            case DataType.Float:
                floatValue = PlayerPrefs.GetFloat(keyName, 0f); // Giá trị mặc định là 0f
                Debug.Log($"Tải {keyName}: {floatValue} (Float)");
                break;
            case DataType.String:
                stringValue = PlayerPrefs.GetString(keyName, ""); // Giá trị mặc định là chuỗi rỗng
                Debug.Log($"Tải {keyName}: {stringValue} (String)");
                break;
        }
    }

    // Hàm kiểm tra xem dữ liệu đã tồn tại chưa
    public bool HasKey()
    {
        return PlayerPrefs.HasKey(keyName);
    }

    // Hàm xóa dữ liệu theo key
    public void DeleteKey()
    {
        if (HasKey())
        {
            PlayerPrefs.DeleteKey(keyName);
            Debug.Log($"Đã xóa key: {keyName}");
        }
        else
        {
            Debug.LogWarning($"Không tìm thấy key: {keyName} để xóa");
        }
    }

    // Hàm set dữ liệu
    public void SetData(string key, int value)
    {
        keyName = key;
        dataType = DataType.Int;
        intValue = value;
    }

    public void SetData(string key, float value)
    {
        keyName = key;
        dataType = DataType.Float;
        floatValue = value;
    }

    public void SetData(string key, string value)
    {
        keyName = key;
        dataType = DataType.String;
        stringValue = value;
    }

    // Hàm lấy dữ liệu
    public int GetInt(string key)
    {
        keyName = key;
        dataType = DataType.Int;
        Load();
        return intValue;
    }

    public float GetFloat(string key)
    {
        keyName = key;
        dataType = DataType.Float;
        Load();
        return floatValue;
    }

    public string GetString(string key)
    {
        keyName = key;
        dataType = DataType.String;
        Load();
        return stringValue;
    }
}
