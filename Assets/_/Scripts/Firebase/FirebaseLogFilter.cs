using UnityEngine;

public class FirebaseLogFilter : MonoBehaviour
{
    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        // Kiểm tra nếu log có chứa thông báo "USE_AUTH_EMULATOR not set"
        if (logString.Contains("USE_AUTH_EMULATOR not set."))
        {
            // Không hiển thị log này
            return;
        }

        if (logString.Contains("The referenced script on this Behaviour (Game Object 'Indoor Volume') is missing!"))
        {
            return;
        }

        if (logString.Contains("The referenced script on this Behaviour (Game Object 'Indoor Volume') is missing!"))
        {
            return;
        }
    }
}
