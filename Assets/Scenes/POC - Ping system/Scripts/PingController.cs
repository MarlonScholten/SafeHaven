using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PingController : MonoBehaviour
{
    [SerializeField] private new Camera camera;
    public GameObject radialMenu;

    [SerializeField] private GameObject markerPrefab;

    //public GameObject interact, use, hide, run;
    public Vector2 inputMouse;

    private Vector3 _pingPosition;
    private const string ActionBrother = "Run";
    private const float Zero = 0f;
    private const float Two = 2f;
    private const float StartingPointCorrection = 90f;
    private const float DegreesHalf = 180f;
    private const float DegreesFull = 360f;

    public Text[] options;
    public Color radialMenuNormal, radialMenuOptionHovered;

    private float _degreesPerSegment;
    public int selectedOption;

    public GameObject highlightedOption;

    private void Start()
    {
    }

    private void Update()
    {
        radialMenu.SetActive(true);

        if (!radialMenu.activeInHierarchy) return;
        inputMouse.x = Mouse.current.position.ReadValue().x - Screen.width / Two;
        inputMouse.y = Mouse.current.position.ReadValue().y - Screen.height / Two;
        inputMouse.Normalize();

        if (inputMouse == Vector2.zero) return;
        float angle = (Mathf.Atan2(inputMouse.y, -inputMouse.x) / Mathf.PI) * DegreesHalf +
                      StartingPointCorrection;

        if (angle < Zero)
        {
            angle += DegreesFull;
        }

        for (var i = 0; i < options.Length; i++)
        {
            _degreesPerSegment = DegreesFull / options.Length;
            if (angle > i * _degreesPerSegment && angle < (i + 1) * _degreesPerSegment)
            {
                options[i].color = radialMenuOptionHovered;
                selectedOption = i;

                highlightedOption.transform.rotation = Quaternion.Euler(0, 0, i * -_degreesPerSegment);
            }
            else
            {
                options[i].color = radialMenuNormal;
            }
        }
    }

    private void OnQuickPing()
    {
        Debug.Log("QUICK PINGGGGG");

        var ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());

        Debug.DrawRay(ray.origin, ray.direction * 10000f, Color.red, 3);
        if (!Physics.Raycast(ray.origin, ray.direction * 10000f, out var hit)) return;

        _pingPosition = hit.point;

        ShowMarker(_pingPosition);

        hit.transform.SendMessage("HitByRay");
        _pingPosition = hit.point;

        Debug.Log(_pingPosition);
    }

    private void ShowMarker(Vector3 position)
    {
        Instantiate(markerPrefab, position, Quaternion.identity);
    }

    private void OnLongPing()
    {
    }

    public Vector3 GetPingLocation()
    {
        return _pingPosition;
    }
}