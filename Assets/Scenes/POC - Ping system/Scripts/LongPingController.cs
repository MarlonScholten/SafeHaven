using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LongPingController : MonoBehaviour
{
    [SerializeField] private GameObject _radialMenu;
    [SerializeField] private GameObject _highlightedOption;

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

    
    private const int Zero = 0;
    private const int One = 1;
    private const int Two = 2;
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

    private void SelectAction()
    {
        Debug.Log("selectaction");
        // _chosenAction
    }

    // private PingType SwitchAction()
    // {
    //     switch (_chosenAction)
    //     {
    //         case PingType.Hide:
    //             break;
    //         case PingType.Interact:
    //             break;
    //         case PingType.Pickup:
    //             break;
    //         case PingType.Run:
    //             break;
    //         case PingType.Use:
    //             break;
    //     }
    //
    //     return _chosenAction;
    // }


    private void Start()
    {
        _radialMenu.SetActive(false);
        // Cursor.visible = false; 
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
        
        if(Enum.TryParse(options[i].text,  out PingType pingType))
        {
            _chosenAction = pingType;
            Debug.Log(_chosenAction);
        }

       // _chosenAction = Enum.TryParse(options[i].text, out PingType pingType) ? pingType : _chosenAction;
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
            // SwitchAction(_);
        }
    }

    private void OnLongPing(InputAction.CallbackContext callbackContext)
    {
        // TODO Move code to OnLongPing method.
        // TODO Ping location.
        // TODO Radial menu disappear.
        // TODO Disable outside when in middle.
        _holdSucceeded = true;
        Debug.Log("performed");
        Time.timeScale /= _slowmotionFactor;
    }

    private void OnLongPingRelease(InputAction.CallbackContext callbackContext)
    {
        Debug.Log("release");
        if (_holdSucceeded)
        {
            _holdSucceeded = false;
            _radialMenuIsSetActive = true;
        }
    }

    public Vector3 GetPingLocation()
    {
        return _pingPosition;
    }
}