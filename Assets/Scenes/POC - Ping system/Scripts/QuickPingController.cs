using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class QuickPingController : MonoBehaviour
{
    //TODO Integrate
    //private BrotherAI _brotherAI;
    
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _markerPrefab;
    [SerializeField] private GameObject _radialMenu;

    private bool _holdSucceeded = false;
    
    private Vector3 _pingPosition;
    private const float Correction = 10000f;

    private bool TFlip = false;
    
    private void Awake()
    {
        _pingSystem = new();
        _pingSystem.Player.QuickPing.performed += OnQuickPing;
        _pingSystem.Player.LongPing.performed += OnLongPingPerformed;
        
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

    private void OnQuickPing(InputAction.CallbackContext callbackContext)
    {
        Debug.Log("onquickping");
        if (!_radialMenu.activeSelf || !_holdSucceeded)
        {
            // if timer is short then long press
            Debug.Log("HIIIII");

            Debug.Log("onquickping");

            if (_radialMenu.activeSelf.Equals(true)) return;
            //|| callbackContext.interaction < 0.2f maxTapduration
            var ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            Debug.DrawRay(ray.origin, ray.direction * Correction, Color.red, 3);

            if (!Physics.Raycast(ray.origin, ray.direction * Correction, out var hit)) return;
            _pingPosition = hit.point;
            ShowMarker(_pingPosition);
            
            //TODO Integrate
            //_brotherAI.PingBrother(PingType.Run, _pingPosition)
        }
        else
        {
            // Timer to check for length press
        }
        TFlip = !TFlip;
    }

    /*private void OnQuickPing(InputAction.CallbackContext callbackContext)
    {
        Debug.Log("onquickping");
        Debug.Log("HEREEEEEEEEE" + callbackContext.interaction.GetType());
        var test = _pingSystem.Player.QuickPing.interactions.Max();
        Debug.Log("OLAAAAA: " + test);
        
        if (_radialMenu.activeSelf.Equals(true) || _holdSucceeded.Equals(true)) return;
        _holdSucceeded = false;
        //|| callbackContext.interaction < 0.2f maxTapduration
        var ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        Debug.DrawRay(ray.origin, ray.direction * Correction, Color.red, 3);

        if (!Physics.Raycast(ray.origin, ray.direction * Correction, out var hit)) return;
        _pingPosition = hit.point;
        ShowMarker(_pingPosition);
        //_pingPosition = hit.point;
            
        //TODO Integrate
        //_brotherAI.PingBrother(PingType.Run, _pingPosition)
    }*/

    private void ShowMarker(Vector3 position)
    {
        Instantiate(_markerPrefab, position, Quaternion.identity);
    }

    public Vector3 GetPingLocation()
    {
        return _pingPosition;
    }
}