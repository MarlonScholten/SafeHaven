using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// Author: Iris Giezen, Thijs Orsel </para>
/// Modified by: n/a </para>
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
    /// Contains a reference to the brother in the current scene, can be set manually.
    /// </summary>
    [SerializeField] protected GameObject _brother;
    
    /// <summary>
    /// Contains a reference to the camera in the current scene, can be set manually.
    /// </summary>
    [SerializeField] protected Camera _camera;
    
    /// <summary>
    /// Contains a reference to the prefab of the marker that is shown when a ping is performed.
    /// </summary>
    [SerializeField] protected GameObject _markerPrefab;
    
    /// <summary>
    /// Contains a reference to the radial menu in the current scene, can be set manually.
    /// </summary>
    [SerializeField] protected GameObject _radialMenu;
    
    /// <summary>
    /// Contains a value that represents the duration in seconds a marker will be visible after a ping, can be set manually.
    /// </summary>
    [SerializeField] private float durationMarkerVisible = 1.0f;

    /// <summary>
    /// Contains a reference to the input actions and is needed for the input system to function.
    /// </summary>
    protected PingSystem _pingSystem;
    
    /// <summary>
    /// Contains the position of the last ping
    /// </summary>
    protected Vector3 _pingPosition;

    /// <summary>
    /// Fetches the reference to the brotherAI script.
    /// </summary>
    protected virtual void Awake()
    {
        _brotherAI = _brother.GetComponent<BrotherAI>();
    }
    
    /// <summary>
    /// Calculates a ray from the camera towards the direction of the mouse.
    /// </summary>
    /// <returns>The ray in the direction of the mouse</returns>
    protected Ray GetRayFromCameraToMousePosition()
    {
        var mousePosition = new Vector2(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y);
        var ray = _camera.ScreenPointToRay(mousePosition);
        return ray;
    }

    /// <summary>
    /// Performs the actual ping and updates the position variable.
    /// </summary>
    /// <param name="ray">The ray that is used to calculate the position of a ping.</param>
    protected void SetPingPosition(Ray ray)
    {
        Debug.DrawRay(ray.origin, ray.direction, Color.red, 3);

        if (!Physics.Raycast(ray.origin, ray.direction, out var hit)) return;
        _pingPosition = hit.point;
    }

    /// <summary>
    /// Timer to destroy a ping marker after a set amount of settings.
    /// </summary>
    /// <param name="marker">Reference to the marker that appeared after a ping.</param>
    /// <returns>Returns a WaitForSeconds</returns>
    protected IEnumerator MarkerDuration(GameObject marker)
    {
        yield return new WaitForSeconds(durationMarkerVisible);
        Destroy(marker);
    }

    /// <summary>
    /// Enables input actions.
    /// </summary>
    protected void OnEnable()
    {
        _pingSystem.Enable();
    }

    /// <summary>
    /// Disables input actions.
    /// </summary>
    protected void OnDisable()
    {
        _pingSystem.Disable();
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
    /// Method that should makes a marker appear.
    /// </summary>
    protected abstract void ShowMarker(Vector3 position);
}