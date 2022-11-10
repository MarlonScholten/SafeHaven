using UnityEngine;
using UnityEngine.InputSystem;

public class PingController : MonoBehaviour
{
    public new Camera camera;

    private Vector3 _pingPosition;
    private const string ActionBrother = "Run";

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnFire()
    {
        var ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());

        Debug.DrawRay(ray.origin, ray.direction * 10000f, Color.red, 3);
        if (Physics.Raycast(ray.origin, ray.direction * 10000f, out var hit))
        {
            hit.transform.SendMessage("HitByRay");

            _pingPosition = hit.point;
            Debug.Log(_pingPosition);
        }
    }

    private void OnQuickPing()
    {
        Debug.Log("QUICK PINGGGGG");
    }
    
    private void OnLongPing()
    {
        Debug.Log("LONG PINGGGGG");
    }

    public Vector3 GetPingLocation()
    {
        return _pingPosition;
    }
}