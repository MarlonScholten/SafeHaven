using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Author: Iris Giezen and Thijs Orsel </para>
/// Modified by: N/A </para>
/// Allows for sending the brother companion the location, which he needs to run to, and shows a marker on the location of the ping for a few seconds.
/// </summary>
/// <list type="table">
///	    <listheader>
///         <term>On what GameObject</term>
///         <term>Type</term>
///         <term>Name of type</term>
///         <term>Description</term>
///     </listheader>
///     <item>
///         <term>Should be placed on the Player.</term>
///		    <term>Script</term>
///         <term>QuickPingController</term>
///		    <term>Allows for sending the brother companion the location, which he needs to run to.</term>
///	    </item>
/// </list>
public class QuickPingController : AbstractPingController
{
    private bool _quickCancelled;
    private GameObject _marker;

    private void Start()
    {
        InputBehaviour.Instance.OnPingQuick += OnQuickPing;
        InputBehaviour.Instance.OnPingQuickCancelled += CancelQuickPing;
        InputBehaviour.Instance.OnPingMenu += StartOnMenuPing;
    }

    /// <summary>
    /// Gets called when left clicking.
    /// Handles the finding and passing the location of the ping.
    /// </summary>
    /// <param name="callbackContext">Information provided to action callbacks about what triggered an action. Needed to make the function react on actions.</param>
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

    /// <summary>
    /// Gets called when a left click is cancelled (done). 
    /// Is used for making sure you can't use QuickPing in the radial menu.
    /// </summary>
    /// <param name="callbackContext">Information provided to action callbacks about what triggered an action. Needed to make the function react on actions.</param>
    private void CancelQuickPing(InputAction.CallbackContext callbackContext)
    {
        _quickCancelled = false;
    }

    /// <summary>
    /// Gets called when a right click is performed.
    /// Is used for making sure you can't use QuickPing in the radial menu.
    /// </summary>
    /// <param name="callbackContext">Information provided to action callbacks about what triggered an action. Needed to make the function react on actions.</param>
    private void StartOnMenuPing(InputAction.CallbackContext callbackContext)
    {
        _quickCancelled = true;
    }
}