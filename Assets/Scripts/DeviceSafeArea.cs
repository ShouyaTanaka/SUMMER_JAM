using UnityEngine;

[ExecuteAlways]
public class DeviceSafeArea : MonoBehaviour
{
    DeviceOrientation device;

    void Update()
    {
        if (Input.deviceOrientation != DeviceOrientation.Unknown && device == Input.deviceOrientation) return;

        device = Input.deviceOrientation;

        var rect = GetComponent<RectTransform>();
        var area = Screen.safeArea;
        var resolution = Screen.currentResolution;

        rect.sizeDelta = Vector2.zero;
        rect.anchorMax = new Vector2(area.xMax / resolution.width, area.yMax / resolution.height);
        rect.anchorMin = new Vector2(area.xMin / resolution.width, area.yMin / resolution.height);
    }
}