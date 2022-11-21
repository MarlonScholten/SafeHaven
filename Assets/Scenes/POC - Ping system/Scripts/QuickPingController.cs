using UnityEngine;
using UnityEngine.InputSystem;

public class QuickPingController : MonoBehaviour
{
    //TODO Integrate
    //private BrotherAI _brotherAI;

    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _markerPrefab;
    [SerializeField] private GameObject _radialMenu;

    private PingSystem _pingSystem;
    private Vector3 _pingPosition;
    private const float Correction = 10000f;

    private bool _quickCancelled;

    private void Awake()
    {
        _pingSystem = new PingSystem();

        _pingSystem.Player.QuickPing.performed += OnQuickPing;
        _pingSystem.Player.QuickPing.canceled += CancelQuickPing;
        _pingSystem.Player.MenuPing.started += StartOnMenuPing;
    }

    private void OnQuickPing(InputAction.CallbackContext callbackContext)
    {
        if (_radialMenu.activeSelf || _quickCancelled) return;
        _quickCancelled = false;

        var ray = GetRayFromCameraToMousePosition();
        SetPingPosition(ray);

        //TODO Integrate
        //_brotherAI.PingBrother(PingType.Run, _pingPosition)
    }
    
    private void CancelQuickPing(InputAction.CallbackContext callbackContext)
    {
        _quickCancelled = false;
    }

    private void StartOnMenuPing(InputAction.CallbackContext callbackContext)
    {
        _quickCancelled = true;
    }

    // Double
    private Ray GetRayFromCameraToMousePosition()
    {
        var mousePosition = new Vector2(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y);
        var ray = _camera.ScreenPointToRay(mousePosition);
        return ray;
    }

    // Double
    private void SetPingPosition(Ray ray)
    {
        Debug.DrawRay(ray.origin, ray.direction * Correction, Color.red, 3);

        if (!Physics.Raycast(ray.origin, ray.direction * Correction, out var hit)) return;
        _pingPosition = hit.point;
        ShowMarker(_pingPosition);
    }

    // Double
    private void ShowMarker(Vector3 position)
    {
        Instantiate(_markerPrefab, position, Quaternion.identity);
    }

    // Double
    private void OnEnable()
    {
        _pingSystem.Enable();
    }

    // Double
    private void OnDisable()
    {
        _pingSystem.Disable();
    }

    // Double
    public Vector3 GetPingLocation()
    {
        return _pingPosition;
    }
}