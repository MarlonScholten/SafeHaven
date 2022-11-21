using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuPingController : MonoBehaviour
{
    //TODO Integrate
    //private BrotherAI _brotherAI;

    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _markerPrefab;
    [SerializeField] private GameObject _radialMenu;
    [SerializeField] private GameObject _highlightedOption;
    [SerializeField] private int _slowmotionFactor = 4;

    private PingSystem _pingSystem;
    private Vector3 _pingPosition;
    private Vector2 _inputMouse;
    private int _selectedOption;

    public Text[] options;
    public Text cancel;
    public Color radialMenuNormal, radialMenuOptionHovered, radialMenuCancel;
    private bool _radialMenuIsSetActive = false;

    private PingType _pingAction;
    private PingType _chosenAction;
    private const string NotCancelled = "Not cancelled";
    private string _cancelled;

    private const float Correction = 10000f;
    private const int StandardTimeFactor = 1;
    private const int Two = 2;

    private const float StartingPointCorrection = 90f;
    private const float DegreesHalf = 180f;
    private const float DegreesFull = 360f;
    private float _degreesPerSegment;

    private const float SizeCircle = 45f;

    private void Awake()
    {
        _pingSystem = new PingSystem();

        _pingSystem.Player.MenuPing.performed += OnMenuPing;
        _pingSystem.Player.QuickPing.performed += OnLeftMouseButton;
    }

    private void Start()
    {
        _radialMenu.SetActive(false);
        _cancelled = NotCancelled;
    }

    private void Update()
    {
        if (!_radialMenuIsSetActive) return;
        ActivateRadialMenu();
    }

    private void ActivateRadialMenu()
    {
        _radialMenu.SetActive(true);

        if (!_radialMenu.activeSelf) return;
        _inputMouse.x = Mouse.current.position.ReadValue().x - Screen.width / Two;
        _inputMouse.y = Mouse.current.position.ReadValue().y - Screen.height / Two;

        if (_inputMouse == Vector2.zero) return;
        var angle = (Mathf.Atan2(_inputMouse.y, -_inputMouse.x) / Mathf.PI) * DegreesHalf + StartingPointCorrection;

        angle = SetDegreesFull(angle);
        ControlSegmentOptions(angle);
    }

    private void ControlSegmentOptions(float angle)
    {
        if (CheckInMiddleSegment())
        {
            ControlSegmentHoveredOverMiddle();
        }
        else
        {
            ControlSegmentHoveredOutside(angle);
        }
    }

    private void ControlSegmentHoveredOverMiddle()
    {
        cancel.color = radialMenuCancel;
        _highlightedOption.SetActive(false);
        options[_selectedOption].color = Color.white;
    }

    private void ControlSegmentHoveredOutside(float angle)
    {
        cancel.color = Color.white;
        for (var i = 0; i < options.Length; i++)
        {
            _degreesPerSegment = DegreesFull / options.Length;
            if (angle > i * _degreesPerSegment && angle < (i + 1) * _degreesPerSegment)
            {
                ControlActionHovered(i);
            }
            else
            {
                options[i].color = radialMenuNormal;
            }
        }
    }

    private void ControlActionHovered(int i)
    {
        options[i].color = radialMenuOptionHovered;
        _selectedOption = i;
        _highlightedOption.SetActive(true);
        _highlightedOption.transform.rotation = Quaternion.Euler(0, 0, i * -_degreesPerSegment);

        if (!Enum.TryParse(options[i].text, out PingType pingType)) return;
        _chosenAction = pingType;
    }

    private void OnLeftMouseButton(InputAction.CallbackContext callbackContext)
    {
        if (!_radialMenu.activeSelf) return;

        DetermineValueCancelled();
        if (!_cancelled.Equals(NotCancelled))
        {
            GetCancel();
        }
        else
        {
            SelectAction();
        }

        CloseRadialMenu();
    }

    private void OnMenuPing(InputAction.CallbackContext callbackContext)
    {
        if (_radialMenuIsSetActive) return;
        _radialMenuIsSetActive = true;
        
        var ray = GetRayFromCameraToMousePosition();
        SetPingPosition(ray);

        Time.timeScale /= _slowmotionFactor;
    }

    private void SelectAction()
    {
        _pingAction = _chosenAction;

        //TODO Integrate
        //_brotherAI.PingBrother(_pingAction, _pingPosition)
    }

    private void CloseRadialMenu()
    {
        _radialMenuIsSetActive = false;
        _radialMenu.SetActive(_radialMenuIsSetActive);

        Time.timeScale = StandardTimeFactor;
    }

    private static float SetDegreesFull(float angle)
    {
        if (!(angle < 0)) return angle;
        angle += DegreesFull;
        return angle;
    }

    private bool CheckInMiddleSegment()
    {
        return _inputMouse.x is < SizeCircle and > -SizeCircle && _inputMouse.y is < SizeCircle and > -SizeCircle;
    }

    private void DetermineValueCancelled()
    {
        _cancelled = CheckInMiddleSegment()
            ? "Cancelled"
            : NotCancelled;
    }
    
    // Double
    private Ray GetRayFromCameraToMousePosition()
    {
        var mousePosition = new Vector2(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y);
        var ray = _camera.ScreenPointToRay(mousePosition);
        return ray;
    }
    
    // Double
    private void SetPingPosition(Ray ray)
    {
        Debug.DrawRay(ray.origin, ray.direction * Correction, Color.red, 3);

        if (!Physics.Raycast(ray.origin, ray.direction * Correction, out var hit)) return;
        _pingPosition = hit.point;
        ShowMarker(_pingPosition);
    }

    // Double
    private void ShowMarker(Vector3 position)
    {
        Instantiate(_markerPrefab, position, Quaternion.identity);
    }

    // Double
    private void OnEnable()
    {
        _pingSystem.Enable();
    }

    // Double
    private void OnDisable()
    {
        _pingSystem.Disable();
    }

    // Double
    public Vector3 GetPingLocation()
    {
        return _pingPosition;
    }

    public PingType GetPingAction()
    {
        return _pingAction;
    }

    public string GetCancel()
    {
        return _cancelled;
    }

    public Tuple<PingType, Vector3> GetPingActionAndLocation()
    {
        return Tuple.Create(GetPingAction(), GetPingLocation());
    }
}