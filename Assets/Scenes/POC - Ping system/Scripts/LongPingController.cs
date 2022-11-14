using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LongPingController : MonoBehaviour
{
    [SerializeField] private GameObject radialMenu;
    [SerializeField] private GameObject highlightedOption;
    [SerializeField] private GameObject cancelOption;

    public Vector2 inputMouse;
    public int selectedOption;

    public Text[] options;
    public Text cancel;
    public Color radialMenuNormal, radialMenuOptionHovered, radialMenuCancel;

    private Vector3 _pingPosition;

    private const int Zero = 0;
    private const int One = 1;
    private const int Two = 2;

    private const float StartingPointCorrection = 90f;
    private const float DegreesHalf = 180f;
    private const float DegreesFull = 360f;
    private float _degreesPerSegment;

    private const float SizeCircle = 45f;

    private void Update()
    {
        radialMenu.SetActive(true);

        if (!radialMenu.activeInHierarchy) return;
        inputMouse.x = Mouse.current.position.ReadValue().x - Screen.width / Two;
        inputMouse.y = Mouse.current.position.ReadValue().y - Screen.height / Two;

        if (inputMouse == Vector2.zero) return;
        var angle = (Mathf.Atan2(inputMouse.y, -inputMouse.x) / Mathf.PI) * DegreesHalf + StartingPointCorrection;

        angle = SetDegreesFull(angle);
        ControlSegmentOptions(angle);
    }

    private void ControlSegmentOptions(float angle)
    {
        if (inputMouse.x is < SizeCircle and > -SizeCircle && inputMouse.y is < SizeCircle and > -SizeCircle)
        {
            cancel.color = radialMenuCancel;
            highlightedOption.SetActive(false);
            options[selectedOption].color = Color.white;
        }
        else
        {
            cancel.color = Color.white;
            for (var i = Zero; i < options.Length; i++)
            {
                _degreesPerSegment = DegreesFull / options.Length;
                if (angle > i * _degreesPerSegment && angle < (i + One) * _degreesPerSegment)
                {
                    ControlSegmentHoveredOver(i);
                }
                else
                {
                    options[i].color = radialMenuNormal;
                }
            }
        }
    }

    private void ControlSegmentHoveredOver(int i)
    {
        options[i].color = radialMenuOptionHovered;
        selectedOption = i;
        highlightedOption.SetActive(true);
        highlightedOption.transform.rotation = Quaternion.Euler(0, 0, i * -_degreesPerSegment);
    }

    private static float SetDegreesFull(float angle)
    {
        if (!(angle < Zero)) return angle;
        angle += DegreesFull;
        return angle;
    }

    private void OnLongPing()
    {
        // TODO Move code to OnLongPing method.
        // TODO Ping location.
        // TODO Text enabled in UI.
        // TODO Radial menu disappear.
        // TODO Disable outside when in middle.
    }

    public Vector3 GetPingLocation()
    {
        return _pingPosition;
    }
}