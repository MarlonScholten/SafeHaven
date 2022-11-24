using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class AbstractPingController : MonoBehaviour
{
    protected BrotherAI _brotherAI;
    
    [SerializeField] private GameObject _brother;
    [SerializeField] protected Camera _camera;
    [SerializeField] protected GameObject _markerPrefab;
    [SerializeField] protected GameObject _radialMenu;
    [SerializeField] private float durationMarkerVisible = 1.0f;

    protected PingSystem _pingSystem;
    protected Vector3 _pingPosition;

    protected virtual void Awake()
    {
        _brotherAI = _brother.GetComponent<BrotherAI>();
    }
    
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
    }

    protected IEnumerator MarkerDuration(GameObject marker)
    {
        yield return new WaitForSeconds(durationMarkerVisible);
        Destroy(marker);
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

    protected abstract void ShowMarker(Vector3 position);
}