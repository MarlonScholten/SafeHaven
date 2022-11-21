using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuPingController : MonoBehaviour
{
    //TODO Integrate
    //private BrotherAI _brotherAI;
    
    [SerializeField] private GameObject _radialMenu;
    [SerializeField] private GameObject _highlightedOption;
    [SerializeField] private GameObject _markerPrefab;
    [SerializeField] private Camera _camera;

    private bool _radialMenuIsSetActive = false;
    private bool _holdSucceeded = false;

    private PingType _chosenAction;

    [SerializeField] private int _slowmotionFactor = 4;

    private PingSystem _pingSystem;
    private Vector2 _inputMouse;
    private int _selectedOption;

    public Text[] options;
    public Text cancel;
    public Color radialMenuNormal, radialMenuOptionHovered, radialMenuCancel;

    private Vector3 _pingPosition;
    private PingType _pingAction;
    private const string NotCancelled = "Not cancelled";
    private string _cancelled;

    private const int Zero = 0;
    private const int One = 1;
    private const int Two = 2;
    private const float Correction = 10000f;
    private const int StandardTimeFactor = 1;

    private const float StartingPointCorrection = 90f;
    private const float DegreesHalf = 180f;
    private const float DegreesFull = 360f;
    private float _degreesPerSegment;

    private const float SizeCircle = 45f;

    private void Awake()
    {
        _pingSystem = new();
        _pingSystem.Player.MenuPing.performed += OnLongPing;
        _pingSystem.Player.MenuPing.canceled += OnMenuPingRelease;
        _pingSystem.Player.Fire.started += OnLeftMouseButton;
    }

    private void Start()
    {
        _radialMenu.SetActive(false);
        _cancelled = NotCancelled;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (_radialMenuIsSetActive)
        {
            ActivateRadialMenu();
        }
    }

    public void OnEnable()
    {
        _pingSystem.Enable();
    }

    public void OnDisable()
    {
        _pingSystem.Disable();
    }

    private void ActivateRadialMenu()
    {
        _radialMenu.SetActive(true);

        if (!_radialMenu.activeInHierarchy) return;
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
            cancel.color = Color.white;
            for (var i = Zero; i < options.Length; i++)
            {
                _degreesPerSegment = DegreesFull / options.Length;
                if (angle > i * _degreesPerSegment && angle < (i + One) * _degreesPerSegment)
                {
                    ControlSegmentHoveredOverOutside(i);
                }
                else
                {
                    options[i].color = radialMenuNormal;
                }
            }
        }
    }

    private bool CheckInMiddleSegment()
    {
        return _inputMouse.x is < SizeCircle and > -SizeCircle && _inputMouse.y is < SizeCircle and > -SizeCircle;
    }

    private void ControlSegmentHoveredOverMiddle()
    {
        cancel.color = radialMenuCancel;
        _highlightedOption.SetActive(false);
        options[_selectedOption].color = Color.white;
    }

    private void ControlSegmentHoveredOverOutside(int i)
    {
        options[i].color = radialMenuOptionHovered;
        _selectedOption = i;
        _highlightedOption.SetActive(true);
        _highlightedOption.transform.rotation = Quaternion.Euler(0, 0, i * -_degreesPerSegment);

        if (Enum.TryParse(options[i].text, out PingType pingType))
        {
            _chosenAction = pingType;
        }
    }

    private static float SetDegreesFull(float angle)
    {
        if (!(angle < Zero)) return angle;
        angle += DegreesFull;
        return angle;
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

    private void CloseRadialMenu()
    {
        _radialMenuIsSetActive = false;
        _radialMenu.SetActive(_radialMenuIsSetActive);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = StandardTimeFactor;
    }

    private void DetermineValueCancelled()
    {
        _cancelled = CheckInMiddleSegment()
            ? "Cancelled"
            : NotCancelled;
    }

    private void SelectAction()
    {
        _pingAction = _chosenAction;

        //TODO Integrate
        //_brotherAI.PingBrother(_pingAction, _pingPosition)
    }

    private void OnLongPing(InputAction.CallbackContext callbackContext)
    {
        _holdSucceeded = true;
        Cursor.lockState = CursorLockMode.None;
        
        if (_radialMenu.activeSelf) return;
        Time.timeScale /= _slowmotionFactor;
    }

    private void ShowMarker(Vector3 position)
    {
        Instantiate(_markerPrefab, position, Quaternion.identity);
    }

    private void OnMenuPingRelease(InputAction.CallbackContext callbackContext)
    {
        if (_radialMenuIsSetActive || !_holdSucceeded) return;
        var ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        Debug.DrawRay(ray.origin, ray.direction * Correction, Color.red, 3);

        if (!Physics.Raycast(ray.origin, ray.direction * Correction, out var hit)) return;
        _pingPosition = hit.point;
        ShowMarker(_pingPosition);

        _holdSucceeded = false;
        _radialMenuIsSetActive = true;
    }

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