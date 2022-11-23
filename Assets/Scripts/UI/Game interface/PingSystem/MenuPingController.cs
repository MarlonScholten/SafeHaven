using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuPingController : AbstractPingController
{

    [SerializeField] private GameObject _highlightedOption;
    [SerializeField] private int _slowmotionFactor = 4;

    private Vector2 _inputMouse;
    private int _selectedOption;
    private GameObject _marker;

    public Text[] options;
    public Text cancel;
    public Color radialMenuNormal, radialMenuOptionHovered, radialMenuCancel;
    private bool _radialMenuIsSetActive = false;

    private PingType _pingAction;
    private PingType _chosenAction;
    private const string NotCancelled = "Not cancelled";
    private string _cancelled;

    private const int StandardTimeFactor = 1;
    private const int Two = 2;

    private const float StartingPointCorrection = 90f;
    private const float DegreesHalf = 180f;
    private const float DegreesFull = 360f;
    private float _degreesPerSegment;

    private const float SizeCircle = 45f;

    protected override void Awake()
    {
        base.Awake();
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
        Debug.Log("radial active? " + _radialMenuIsSetActive);
        if (!_radialMenuIsSetActive) return;
        Debug.Log("update");
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
        ShowMarker(_pingPosition);

        Time.timeScale /= _slowmotionFactor;
    }

    private void SelectAction()
    {
        _pingAction = _chosenAction;
        
        _brotherAI.PingBrother(_pingAction, _pingPosition);
    }

    private void CloseRadialMenu()
    {
        _radialMenuIsSetActive = false;
        _radialMenu.SetActive(_radialMenuIsSetActive);

        StartCoroutine(MarkerDuration(_marker));
        Time.timeScale = StandardTimeFactor;
    }

    protected override void ShowMarker(Vector3 position)
    {
        _marker = Instantiate(_markerPrefab, position, Quaternion.identity);
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