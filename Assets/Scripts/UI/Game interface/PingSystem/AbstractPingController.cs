using UnityEngine;
using UnityEngine.InputSystem;

public abstract class AbstractPingController : MonoBehaviour
{
    //TODO Integrate
    //private BrotherAI _brotherAI;

    [SerializeField] protected Camera _camera;
    [SerializeField] protected GameObject _markerPrefab;
    [SerializeField] protected GameObject _radialMenu;
    
    protected PingSystem _pingSystem;
    protected Vector3 _pingPosition;

    protected Ray GetRayFromCameraToMousePosition()
    {
        var mousePosition = new Vector2(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y);
        var ray = _camera.ScreenPointToRay(mousePosition);
        return ray;
    }

    protected void SetPingPosition(Ray ray)
    {
        Debug.DrawRay(ray.origin, ray.direction, Color.red, 3);

        if (!Physics.Raycast(ray.origin, ray.direction, out var hit)) return;
        _pingPosition = hit.point;
        //ShowMarker(_pingPosition);
    }

    protected void OnEnable()
    {
        _pingSystem.Enable();
    }

    protected void OnDisable()
    {
        _pingSystem.Disable();
    }

    protected Vector3 GetPingLocation()
    {
        return _pingPosition;
    }
}
