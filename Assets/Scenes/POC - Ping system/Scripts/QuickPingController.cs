using UnityEngine;
using UnityEngine.InputSystem;

public class QuickPingController : AbstractPingController
{
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
}