using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using UnityEngine.Networking;
using TMPro;


public class ChatbotAI : MonoBehaviour
{
    public InputField inputField; 
    public TextMeshProUGUI textDisplay; 

    private string url = "https://eternal-odyssey.mekongai.com/api/v1/chatbot-stream";

    private void Start()
    {
        // Gắn sự kiện onEndEdit
        inputField.onEndEdit.AddListener(HandleInput);
    }

    public void CallAPI(string query)
    {
        StartCoroutine(StreamText(query));
    }

    private IEnumerator StreamText(string query)
    {
        // Xóa nội dung cũ
        textDisplay.text = "";

        // Tạo body JSON
        string jsonBody = "{ \"query\": \"" + query + "\" }";
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);

        // Thiết lập UnityWebRequest
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Bắt đầu gửi request
        request.timeout = 0; // Không giới hạn thời gian chờ
        request.SendWebRequest();

        // Dữ liệu đã xử lý
        int previousLength = 0; // Lưu độ dài dữ liệu đã xử lý

        // Kiểm tra liên tục dữ liệu đến
        while (!request.isDone || request.downloadHandler.text.Length > previousLength)
        {
            // Lấy dữ liệu từ downloadHandler
            string currentText = request.downloadHandler.text;

            if (!string.IsNullOrEmpty(currentText) && currentText.Length > previousLength)
            {
                // Tính toán dữ liệu mới
                string newText = currentText.Substring(previousLength);

                // Cập nhật từng ký tự
                foreach (char c in newText)
                {
                    textDisplay.text += c;
                    yield return null; // Hiển thị ngay lập tức
                }

                // Cập nhật trạng thái dữ liệu đã đọc
                previousLength = currentText.Length;
            }

            yield return null; // Chờ chunk tiếp theo
        }

        // Kiểm tra hoàn tất
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Stream hoàn tất!");
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }

        inputField.text = "";
    }


    private void HandleInput(string text)
    {
        // Kiểm tra nếu nhấn Enter
        if (Input.GetKeyDown(KeyCode.Return))
        {
            CallAPI(text); // Truyền text vào API
        }
    }

    private void OnDestroy()
    {
        // Hủy sự kiện để tránh lỗi khi đối tượng bị hủy
        inputField.onEndEdit.RemoveListener(HandleInput);
    }
}
