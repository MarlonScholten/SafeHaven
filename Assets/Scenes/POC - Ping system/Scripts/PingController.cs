using UnityEngine;
using UnityEngine.InputSystem;

public class PingController : MonoBehaviour
{
    public new Camera camera;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnFire()
    {
        var ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());

        Debug.DrawRay(ray.origin, ray.direction * 10000f, Color.red, 3);
        if (Physics.Raycast(ray.origin, ray.direction * 10000f, out var hit))
        {
            hit.transform.SendMessage("HitByRay");

            Debug.Log(hit.transform);
        }
    }
}