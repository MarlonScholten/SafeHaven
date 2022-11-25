using UnityEngine;

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
