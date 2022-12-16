using PlayerCharacter.Movement;
using UnityEngine;

/// <summary>
/// Author: Iris Giezen and Thijs Orsel </para>
/// Modified by: Marlon Scholten </para>
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
    /// <summary>
    /// Stores if the QuickPing is cancelled
    /// </summary>
    private bool _quickCancelled;
    
    /// <summary>
    /// Stores the reference to the instance of the marker of a ping.
    /// </summary>
    private GameObject _marker;

    private void Start()
    {
        InputBehaviour.Instance.OnPingQuickEvent += OnQuickPing;
        InputBehaviour.Instance.OnPingQuickCancelledEvent += CancelQuickPing;
        InputBehaviour.Instance.OnPingMenuEvent += StartOnMenuPing;
    }

    /// <summary>
    /// Gets called when left clicking.
    /// Handles the finding and passing the location of the ping.
    /// </summary>
    private void OnQuickPing()
    {
        if (_radialMenu.activeSelf || _quickCancelled) return;
        _playerRayCastHit = GetComponent<PlayerController>().CamRayCastHit;
        if (_playerRayCastHit.point == Vector3.zero) return;
        _quickCancelled = false;

        SetPingPosition(_playerRayCastHit.point);
        ShowMarker(_pingPosition);

        _brotherAI.PingBrother(PingType.Move, _pingPosition);
    }

    /// <summary>
    /// Shows a marker on the location of a ping, this lasts a given amount of seconds.
    /// </summary>
    /// <param name="position">The position the marker should be shown at.</param>
    protected override void ShowMarker(Vector3 position)
    {
        if (_marker) Destroy(_marker);
        
        _marker = Instantiate(_markerPrefab, position, Quaternion.identity);
        StartCoroutine(MarkerDuration(_marker));
    }

    /// <summary>
    /// Gets called when a left click is cancelled (done). 
    /// Is used for making sure you can't use QuickPing in the radial menu.
    /// </summary>
    private void CancelQuickPing()
    {
        _quickCancelled = false;
    }

    /// <summary>
    /// Gets called when a right click is performed.
    /// Is used for making sure you can't use QuickPing in the radial menu.
    /// </summary>
    private void StartOnMenuPing()
    {
        _quickCancelled = true;
    }
}