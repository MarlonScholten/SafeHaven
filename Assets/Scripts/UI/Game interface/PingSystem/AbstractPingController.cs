using System.Collections;
using PlayerCharacter.Movement;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Author: Iris Giezen and Thijs Orsel </para>
/// Modified by: N/A </para>
/// Abstract class to prevent duplicate functionality in both of the ping controllers (QuickPingController and MenuPingController).
/// </summary>
/// <list type="table">
///	    <listheader>
///         <term>On what GameObject</term>
///         <term>Type</term>
///         <term>Name of type</term>
///         <term>Description</term>
///     </listheader>
///     <item>
///         <term>None</term>
///		    <term>Script</term>
///         <term>AbstractPingController</term>
///		    <term>Abstract class to prevent duplicate functionality in the actual ping controllers.</term>
///	    </item>
/// </list>
public abstract class AbstractPingController : MonoBehaviour
{
    /// <summary>
    /// Contains a reference to the brotherAI script of the brother in the current scene.
    /// </summary>
    protected BrotherAI _brotherAI;

    /// <summary>
    /// Contains a reference to the brother in the current scene, which can be set manually.
    /// </summary>
    [Tooltip("The reference to the brother.")]
    [SerializeField] protected GameObject _brother;

    /// <summary>
    /// Contains a reference to the camera in the current scene, which can be set manually.
    /// </summary>
    [Tooltip("The reference to the camera.")]
    [SerializeField] protected Camera _camera;

    /// <summary>
    /// Contains a reference to the prefab of the marker that is shown when a ping is performed.
    /// </summary>
    [Tooltip("The reference to the prefab of the marker.")]
    [SerializeField] protected GameObject _markerPrefab;
    
    /// <summary>
    /// Contains a reference to the prefab of the marker that is shown when a hide ping is performed.
    /// </summary>
    [Tooltip("The reference to the prefab of the marker.")]
    [SerializeField] protected GameObject _markerPrefabHide;
    
    /// <summary>
    /// Contains a reference to the prefab of the marker that is shown when a interact ping is performed.
    /// </summary>
    [Tooltip("The reference to the prefab of the marker.")]
    [SerializeField] protected GameObject _markerPrefabInteract;

    /// <summary>
    /// Contains a reference to the radial menu in the current scene, which can be set manually.
    /// </summary>
    [Tooltip("The reference to the radial menu.")]
    [SerializeField] protected GameObject _radialMenu;

    /// <summary>
    /// Contains a value that represents the duration in seconds a marker will be visible after a ping, which can be set manually.
    /// </summary>
    [Tooltip("The duration of the visibility from the marker.")]
    [SerializeField] private float durationMarkerVisible = 0.1f;

    /// <summary>
    /// Contains the position of the last ping.
    /// </summary>
    protected Vector3 _pingPosition;

    /// <summary>
    /// The PlayerController's raycasthit to see what the player is looking at.
    /// </summary>
    protected RaycastHit _playerRayCastHit;

    /// <summary>
    /// Fetches the reference to the brotherAI script.
    /// </summary>
    protected virtual void Awake()
    {
        _brotherAI = _brother.GetComponent<BrotherAI>();
        _playerRayCastHit = GetComponent<PlayerController>().CamRayCastHit;
    }

    /// <summary>
    /// Performs the actual ping and updates the position variable.
    /// </summary>
    protected void SetPingPosition(Vector3 point)
    {
        _pingPosition = point;
    }

    /// <summary>
    /// Timer to destroy a ping marker after the brother reached the ping position.
    /// </summary>
    /// <param name="marker">Reference to the marker that appeared after a ping.</param>
    /// <returns>Returns a WaitForSeconds.</returns>
    protected IEnumerator MarkerDuration(GameObject marker)
    {
        yield return new WaitForSeconds(durationMarkerVisible);
        //check if the player is at destination, if he is not, keep the marker
        while (!_brotherAI.PathCompleted())
        {
            yield return null;
        }
        Destroy(marker);
    }

    /// <summary>
    /// Used to get the ping location.
    /// </summary>
    /// <returns>Ping location.</returns>
    protected Vector3 GetPingLocation()
    {
        return _pingPosition;
    }

    /// <summary>
    /// Method that should make a marker appear.
    /// </summary>
    protected abstract void ShowMarker(Vector3 position);
}