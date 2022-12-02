using UnityEngine;

/// <summary>
/// Author: Hugo Verweij <br/>
/// Modified by: - <br/>
/// RouteTrigger script. Handles the on trigger execution from the <see cref="RouteBehaviour"/>. <br/>
/// </summary>
/// <list type="table">
///	    <listheader>
///         <term>On what GameObject</term>
///         <term>Type</term>
///         <term>Name of type</term>
///         <term>Description</term>
///     </listheader>
///	    <item>
///         <term>Route prefab.</term>
///		    <term>Script</term>
///         <term>RouteTrigger</term>
///		    <term>RouteTrigger script. Handles the on trigger execution from the <see cref="RouteBehaviour"/>.</term>
///	    </item>
/// </list>
public class RouteTrigger : MonoBehaviour
{
    [SerializeField] private RouteBehaviour _route;

    private bool _used;

    private void OnTriggerEnter(Collider other)
    {
        // Return if not triggered by the player or if it's already used.
        if (!other.CompareTag("Player") || _used)
            return;

        // Lock & start the coroutine.
        _used = true;
        StartCoroutine(_route.RouteCoroutine(RouteBehaviour.ExecuteType.OnTrigger));
    }
}
