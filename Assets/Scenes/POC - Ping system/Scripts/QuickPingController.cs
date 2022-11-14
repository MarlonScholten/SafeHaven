using UnityEngine;
using UnityEngine.InputSystem;

public class QuickPingController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _markerPrefab;

    private Vector3 _pingPosition;
    private const float Correction = 10000f;

    private void OnQuickPing()
    {
        var ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        Debug.DrawRay(ray.origin, ray.direction * Correction, Color.red, 3);

        if (!Physics.Raycast(ray.origin, ray.direction * Correction, out var hit)) return;
        _pingPosition = hit.point;
        ShowMarker(_pingPosition);
        _pingPosition = hit.point;
    }

    private void ShowMarker(Vector3 position)
    {
        Instantiate(_markerPrefab, position, Quaternion.identity);
    }

    public Vector3 GetPingLocation()
    {
        return _pingPosition;
    }
}