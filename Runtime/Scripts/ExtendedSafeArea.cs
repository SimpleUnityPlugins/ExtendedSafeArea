using System;
using UnityEngine;

public class ExtendedSafeArea : MonoBehaviour {
    [Serializable]
    public enum SafeAreaType {
        FullScreen,
        Width,
        Height
    }

    [SerializeField] private SafeAreaType safeAreaType;
    private RectTransform _panel;
    private Rect _lastSafeArea = new(0, 0, 0, 0);

    private void Awake() {
        _panel = GetComponent<RectTransform>();
        Refresh();
    }

    private void Update() {
        Refresh();
    }

    private void Refresh() {
        var safeArea = GetSafeArea();

        if (safeArea != _lastSafeArea)
            ApplySafeArea(safeArea);
    }

    private static Rect GetSafeArea() {
        return Screen.safeArea;
    }


    private void ApplySafeArea(Rect r) {
        _lastSafeArea = r;

        var anchorMin = r.position;
        var anchorMax = r.position + r.size;


        switch (safeAreaType) {
            case SafeAreaType.Width:
                anchorMin.x /= Screen.width;
                anchorMin.y = _panel.anchorMin.y;
                anchorMax.x /= Screen.width;
                anchorMax.y = _panel.anchorMax.y;
                break;
            case SafeAreaType.Height:
                anchorMin.x = _panel.anchorMin.x;
                anchorMin.y /= Screen.height;
                anchorMax.x = _panel.anchorMax.x;
                anchorMax.y /= Screen.height;
                break;
            case SafeAreaType.FullScreen:
            default:
                anchorMin.x /= Screen.width;
                anchorMin.y /= Screen.height;
                anchorMax.x /= Screen.width;
                anchorMax.y /= Screen.height;
                break;
        }


        _panel.anchorMin = anchorMin;
        _panel.anchorMax = anchorMax;

        Debug.LogFormat("New safe area applied to {0}: x={1}, y={2}, w={3}, h={4} on full extents w={5}, h={6}", name, r.x, r.y, r.width, r.height,
            Screen.width, Screen.height);
    }
}