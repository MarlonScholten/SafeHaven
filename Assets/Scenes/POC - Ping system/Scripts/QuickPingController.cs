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

    private bool _holdSucceeded = false;

    private Vector3 _pingPosition;
    private const float Correction = 10000f;

    private void Awake()
    {
        _pingSystem = new PingSystem();
        _pingSystem.Player.QuickPing.performed += OnQuickPing;
        _pingSystem.Player.LongPing.performed += OnLongPingPerformed;
        _pingSystem.Player.Fire.started += OnLeftMouseButton;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnEnable()
    {
        _pingSystem.Enable();
    }

    public void OnDisable()
    {
        _pingSystem.Disable();
    }

    private void OnLongPingPerformed(InputAction.CallbackContext callbackContext)
    {
        _holdSucceeded = true;
    }

    private void OnLeftMouseButton(InputAction.CallbackContext callbackContext)
    {
        if (!_radialMenu.activeSelf) return;
        _holdSucceeded = false;
    }

    public float aanpassss;
    private void OnQuickPing(InputAction.CallbackContext callbackContext)
    {
        Debug.Log("onquickping");
        Debug.Log("radialactive " + _radialMenu.activeSelf + "holdscucceed " + _holdSucceeded);
        if (_radialMenu.activeSelf || _holdSucceeded) return;
        if (_radialMenu.activeSelf.Equals(true)) return;

        var mousePostion = Mouse.current.position.ReadValue();
        var newMousePosition = mousePostion;
        var ray = _camera.ScreenPointToRay( newMousePosition);
        Debug.DrawRay(ray.origin, ray.direction * Correction, Color.red, 3);

        if (!Physics.Raycast(ray.origin, ray.direction * Correction, out var hit)) return;
        _pingPosition = hit.point;
        ShowMarker(_pingPosition);
            
        //TODO Integrate
        //_brotherAI.PingBrother(PingType.Run, _pingPosition)
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