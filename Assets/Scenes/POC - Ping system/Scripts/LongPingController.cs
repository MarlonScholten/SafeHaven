using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LongPingController : MonoBehaviour
{
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
        _pingSystem.Player.LongPing.performed += OnLongPing;
        _pingSystem.Player.LongPing.canceled += OnLongPingRelease;
        _pingSystem.Player.Fire.started += OnLeftMouseButton;
    }

    private void Start()
    {
        _radialMenu.SetActive(false);
        //TODO: Cursor.lockState = CursorLockMode.Locked;
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

        //radialMenu.SetActive(false);
    }

    private void ControlSegmentOptions(float angle)
    {
        if (_inputMouse.x is < SizeCircle and > -SizeCircle && _inputMouse.y is < SizeCircle and > -SizeCircle)
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
           // Debug.Log(_chosenAction);
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
        if (_radialMenu.activeSelf)
        {
            SelectAction();
            _radialMenuIsSetActive = false;
            _radialMenu.SetActive(false);
            Time.timeScale = StandardTimeFactor;
        }
    }

    private void SelectAction()
    {
        _pingAction = _chosenAction;
        
    }

    private void OnLongPing(InputAction.CallbackContext callbackContext)
    {
        // TODO Disable outside when in middle.?
        _holdSucceeded = true;
        // TODO: Cursor.lockState = CursorLockMode.None;
        Debug.Log("performed");
        Time.timeScale /= _slowmotionFactor;
    }

    private void ShowMarker(Vector3 position)
    {
        Instantiate(_markerPrefab, position, Quaternion.identity);
    }

    private void OnLongPingRelease(InputAction.CallbackContext callbackContext)
    {
        Debug.Log("release");
        if (_holdSucceeded)
        {
            var ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            Debug.DrawRay(ray.origin, ray.direction * Correction, Color.red, 3);

            if (!Physics.Raycast(ray.origin, ray.direction * Correction, out var hit)) return;
            _pingPosition = hit.point;
            ShowMarker(_pingPosition);
            _pingPosition = hit.point;

            _holdSucceeded = false;
            _radialMenuIsSetActive = true;
        }
    }

    public Vector3 GetPingLocation()
    {
        return _pingPosition;
    }

    public PingType GetPingAction()
    {
        return _pingAction;
    }
}