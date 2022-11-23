using UnityEngine;
using UnityEngine.InputSystem;

public class QuickPingController : AbstractPingController
{
    private bool _quickCancelled;
    private GameObject _marker;

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
        ShowMarker(_pingPosition);

        _brotherAI.PingBrother(PingType.Run, _pingPosition);
    }

    protected override void ShowMarker(Vector3 position)
    { 
        _marker = Instantiate(_markerPrefab, position, Quaternion.identity);
        StartCoroutine(MarkerDuration(_marker));
    }

    private void CancelQuickPing(InputAction.CallbackContext callbackContext)
    {
        _quickCancelled = false;
    }

    private void StartOnMenuPing(InputAction.CallbackContext callbackContext)
    {
        _quickCancelled = true;
    }
}