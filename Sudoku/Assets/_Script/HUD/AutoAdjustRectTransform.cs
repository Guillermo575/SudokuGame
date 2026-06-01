using UnityEngine;
[RequireComponent(typeof(RectTransform))]
public class AutoAdjustRectTransform : MonoBehaviour
{
    public Vector2 sizeInLandscape = new Vector2(200, 100);
    public Vector2 sizeInPortrait = new Vector2(100, 200);
    private RectTransform rectTransform;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        UpdateSize();
    }
    void Update()
    {
        if (rectTransform == null) return;
        UpdateSize();
    }
    void UpdateSize()
    {
        rectTransform.sizeDelta = Screen.width > Screen.height ? sizeInLandscape : sizeInPortrait;
    }
}