using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public string itemId;
    private Transform originalParent;  // Lưu vị trí gốc của Item trước khi kéo
    private CanvasGroup canvasGroup;   // Dùng để thay đổi alpha của item khi kéo
    private RectTransform rectTransform;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    // Sự kiện bắt đầu kéo
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;  // Lưu lại vị trí gốc
        transform.SetParent(transform.root);  // Đưa item ra khỏi Grid để kéo tự do
        canvasGroup.blocksRaycasts = false;  // Để cho phép item được kéo qua các slot khác
    }

    // Sự kiện khi đang kéo
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvasGroup.transform.localScale.x;
    }

    // Sự kiện khi kết thúc kéo
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // Nếu không thả vào Slot hợp lệ, quay về vị trí gốc
        if (transform.parent == transform.root)
        {
            transform.SetParent(originalParent);
            transform.localPosition = Vector3.zero;
        }
    }
}
