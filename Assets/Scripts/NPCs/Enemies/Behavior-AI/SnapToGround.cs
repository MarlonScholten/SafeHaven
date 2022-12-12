using UnityEngine;

/// <summary>
/// Attach this script as a component to a GameObject to snap it to the ground
/// </summary>
[ExecuteInEditMode]
public class SnapToGround : MonoBehaviour
{
    [SerializeField] 
    [Tooltip("The maximum distance that the GameObject will move up to snap to a surface")]
    private float _maxSnapDistanceUp = 0.2f;
    
    [SerializeField] 
    [Tooltip("The maximum distance that the GameObject will move down to snap to a surface")]
    private float _maxSnapDistanceDown = 3.0f;
    
    private Vector3 _oldPosition;
    
    void Update()
    {
        if (_oldPosition == transform.position) return;

        if (!Physics.Raycast(
                transform.position + new Vector3(0.0f, _maxSnapDistanceUp, 0.0f), 
                transform.up * -1, 
                out var hit, 
                _maxSnapDistanceDown)
            ) return;
        
        transform.position = hit.point;
        _oldPosition = transform.position;
    }
}
