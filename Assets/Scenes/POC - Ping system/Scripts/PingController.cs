using System.Globalization;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PingController : MonoBehaviour
{
    [SerializeField]
    private new Camera camera;
    public GameObject radialMenu;

    [SerializeField]
    private GameObject markerPrefab;
    //public GameObject interact, use, hide, run;
    public Vector2 inputMouse;

    private Vector3 _pingPosition;
    private const string ActionBrother = "Run";
    private const float TWO = 2f;

    private void Start()
    {
    }

    private void Update()
    {
    }

    private void OnQuickPing()
    {
        Debug.Log("QUICK PINGGGGG");

        var ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());

        Debug.DrawRay(ray.origin, ray.direction * 10000f, Color.red, 3);
        if (!Physics.Raycast(ray.origin, ray.direction * 10000f, out var hit)) return;
        
        hit.transform.SendMessage("HitByRay");
        _pingPosition = hit.point;
        
        ShowMarker(_pingPosition);
        
        
        Debug.Log(_pingPosition);
    }

    private void ShowMarker(Vector3 position)
    {
        Instantiate(markerPrefab, position, Quaternion.identity);
    }

    private void OnLongPing()
    {
        Debug.Log("LONG PINGGGGG");
        
        radialMenu.SetActive(true);

        //if (radialMenu.activeInHierarchy)
        //{
            inputMouse.x = Mouse.current.position.ReadValue().x - Screen.width / TWO;
            inputMouse.y = Mouse.current.position.ReadValue().y - Screen.height / TWO;
            inputMouse.Normalize();
            
            Debug.Log(inputMouse);

            if (inputMouse != Vector2.zero)
            {
                float angle = (Mathf.Atan2(inputMouse.y, -inputMouse.x) / Mathf.PI) * 180f + 90f;

                if (angle < 0)
                {
                    angle += 360;
                }
            }
        //}
    }

    public Vector3 GetPingLocation()
    {
        return _pingPosition;
    }
}