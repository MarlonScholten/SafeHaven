using System;
using Cinemachine;
using PlayerCharacter.Movement;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

/// <summary>
/// Author: Thijs Orsel and Iris Giezen </para>
/// Modified by: N/A </para>
/// This script controls the radial menu for the pinging system.
/// In order to let it work, the 'Radial Menu' prefab should be added in the hierarchy.
/// It places a marker on the pinged location.
/// It also opens and closes the radial menu and makes it possible to interact with it by the player.
/// </summary>
/// <list type="table">
///	    <listheader>
///         <term>On what GameObject</term>
///         <term>Type</term>
///         <term>Name of type</term>
///         <term>Description</term>
///     </listheader>
///     <item>
///         <term>Should be placed on the player (sister).</term>
///		    <term>Script</term>
///         <term>MenuPingController</term>
///		    <term>The script makes it possible that the player is able to let the little brother (Brother AI) perform different actions with the help of the radial menu.</term>
///	    </item>
/// </list>
public class MenuPingController : AbstractPingController
{
    /// <summary>
    /// When the radial menu is active the player can hover over different actions.
    /// The segment, on which the player hovers, changes color.
    /// In here should be the Segment Outside, which contains all the segments that are available in the radial menu. 
    /// </summary>
    [Tooltip("The option on which the player hovers.")]
    [SerializeField] private GameObject _highlightedOption;

    /// <summary>
    /// When the radial menu is open time slows down in game, this factor provides the ability to tweak how much time slows down.
    /// </summary>
    [Tooltip("The factor on how slow the time goes when the radial menu is active.")]
    [SerializeField] private int _slowmotionFactor = 4;

    /// <summary>
    /// Different parts of the radial menu have varying colors.
    /// The radialMenuNormal controls the color of the text in the segments of the radial menu when nothing hovers over it.
    /// The radialMenuOptionHovered controls the color of the text in the outside segment of the radial menu when the mouse hovers over it.
    /// The radialMenuCancel controls the color of the text in the middle segment of the radial menu when the mouse hovers over it.
    /// </summary>
    [Tooltip("The colors of the UI Text elements.")]
    [SerializeField] private Color _radialMenuNormal, _radialMenuOptionHovered, _radialMenuCancel;

    /// <summary>
    /// In the radial menu are different sections.
    /// These are the different Text objects that are present in the radial menu.
    /// </summary>
    [Tooltip("The options in the different segments.")]
    [SerializeField] private Text[] _options;

    /// <summary>
    /// In the middle of the radial menu is a cancel functionality.
    /// In here is the Text object from the cancel option.
    /// </summary>
    [Tooltip("The Text object for the cancel option.")]
    [SerializeField] private GameObject _cancel;

    /// <summary>
    /// Vector2 variable that stores the input values of the mouse.
    /// </summary>
    private Vector2 _inputMouse;

    /// <summary>
    /// Stores the reference number of the current selected option in the radial menu.
    /// </summary>
    private int _selectedOption;

    /// <summary>
    /// Stores the reference to the instance of the marker of a ping.
    /// </summary>
    private GameObject _marker;

    /// <summary>
    /// Stores whether the radial menu is active.
    /// </summary>
    private bool _radialMenuIsSetActive = false;

    /// <summary>
    /// Stores the currently selected action.
    /// </summary>
    private PingType _pingAction;

    /// <summary>
    /// Stores the final selected action.
    /// </summary>
    private PingType _chosenAction;

    /// <summary>
    /// Stores the size of the radial menu segments.
    /// </summary>
    private float _degreesPerSegment;

    /// <summary>
    /// Contains if the menu has been cancelled.
    /// </summary>
    private string _cancelled;

    private const int StandardTimeFactor = 1;
    private const int Two = 2;
    private const float StartingPointCorrection = 90f;
    private const float DegreesHalf = 180f;
    private const float DegreesFull = 360f;

    /// <summary>
    /// Contains that the menu has been cancelled.
    /// </summary>
    private const string NotCancelled = "Not cancelled";

    /// <summary>
    /// Stores the size of the cancel button.
    /// </summary>
    private const float SizeCircle = 45f;

    private CinemachineCore.AxisInputDelegate _cameraInput;

    private Image _image;
    
    private Vector2 _previousMousePosition = new Vector2(Screen.width / 2,Screen.height / 2);

    /// <summary>
    /// De-Activates the radial menu and initializes the input events.
    /// </summary>
    private void Start()
    {
        _radialMenu.SetActive(false);
        _cancelled = NotCancelled;
        InputBehaviour.Instance.OnPingMenuEvent += OnMenuPing;
        InputBehaviour.Instance.OnPingQuickEvent += OnLeftMouseButton;
        
        _image = _cancel.GetComponent<Image>();
    }

    /// <summary>
    /// Shows the radial menu when activated.
    /// </summary>
    private void Update()
    {
        if (!_radialMenuIsSetActive) return;
        ActivateRadialMenu();
        ConstrainMouse();

    }

    private void ConstrainMouse()
    {
        if (Time.timeScale <= 0) return;
        
        Vector2 screenCenter = new Vector2(Screen.width / 2,Screen.height / 2);
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        if (Vector2.Distance(screenCenter, mousePosition) > Screen.width/4.5)
        {
            Vector2 warpPosition = _previousMousePosition; 
            warpPosition = Vector2.MoveTowards(warpPosition, screenCenter, 10);
            Mouse.current.WarpCursorPosition(warpPosition);
            InputState.Change(Mouse.current.position, warpPosition);
        }
        _previousMousePosition = Mouse.current.position.ReadValue();
    }
    
    /// <summary>
    /// Used to activate the radial menu.
    /// </summary>
    private void ActivateRadialMenu()
    {
        _radialMenu.SetActive(true);

        if (!_radialMenu.activeSelf) return;
        Input.mousePosition.Set(Screen.width / 2, Screen.height / 2, 0);
        _inputMouse.x = Mouse.current.position.ReadValue().x - Screen.width / Two;
        _inputMouse.y = Mouse.current.position.ReadValue().y - Screen.height / Two;

        if (_inputMouse == Vector2.zero) return;
        var angle = (Mathf.Atan2(_inputMouse.y, -_inputMouse.x) / Mathf.PI) * DegreesHalf + StartingPointCorrection;

        angle = SetDegreesFull(angle);
        ControlSegmentOptions(angle);
    }

    /// <summary>
    /// Decides what segment is hovered.
    /// </summary>
    /// <param name="angle">The angle of the radial menu the mouse is at.</param>
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

    /// <summary>
    /// Highlights the middle of the radial menu.
    /// </summary>
    private void ControlSegmentHoveredOverMiddle()
    {
        _image.color = _radialMenuCancel;


           _highlightedOption.SetActive(false);
        
        //image.GetComponent<Image>().color = new Color32(255,255,225,100);

        foreach (var action in _options)
        {
            action.color = _radialMenuNormal;
        }
    }

    /// <summary>
    /// Highlights the selected segment of the radial menu.
    /// </summary>
    private void ControlSegmentHoveredOutside(float angle)
    {
        _image.color = Color.white;
        for (var i = 0; i < _options.Length; i++)
        {
            _degreesPerSegment = DegreesFull / _options.Length;
            if (angle > i * _degreesPerSegment && angle < (i + 1) * _degreesPerSegment)
            {
                ControlActionHovered(i);
            }
            else
            {
                _options[i].color = _radialMenuNormal;
            }
        }
    }

    /// <summary>
    /// Highlights the selected segment of the radial menu.
    /// </summary>
    /// <param name="i">The segment that should be affected.</param>
    private void ControlActionHovered(int i)
    {
        _options[i].color = _radialMenuOptionHovered;
        _selectedOption = i;
        _highlightedOption.SetActive(true);
        _highlightedOption.transform.rotation = Quaternion.Euler(0, 0, i * -_degreesPerSegment);

        if (!Enum.TryParse(_options[i].text, out PingType pingType)) return;
        _chosenAction = pingType;
    }

    /// <summary>
    /// When the player presses the left mouse button an action can be selected or the radial menu can be cancelled.
    /// </summary>
    private void OnLeftMouseButton()
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

    /// <summary>
    /// When the player presses the right mouse button a ping gets placed, time slows down and the radial menu gets activated.
    /// </summary>
    private void OnMenuPing()
    {
        if (_radialMenuIsSetActive)
        {
            CloseRadialMenu();
        }
        else
        {
            _playerRayCastHit = GetComponent<PlayerController>().CamRayCastHit;
            if (_playerRayCastHit.point == Vector3.zero) return;
            _radialMenuIsSetActive = true;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;

            SetPingPosition(_playerRayCastHit.point);

            Time.timeScale /= _slowmotionFactor;

            // Override camera input with 0 and save old behaviour
            _cameraInput = CinemachineCore.GetInputAxis;
            CinemachineCore.GetInputAxis = axisName => 0;
        }
    }

    /// <summary>
    /// The chosen action from the player gets set to the ping action.
    /// The ping action together with the ping position will be passed on to the Brother AI.
    /// This is the place where the integration with the Brother AI takes place.
    /// </summary>
    private void SelectAction()
    {
        _pingAction = _chosenAction;
        ShowMarker(_pingPosition);
        _brotherAI.PingBrother(_pingAction, _pingPosition);
    }

    /// <summary>
    /// Closes the radial radial menu and makes the marker disappear after a set amount of seconds.
    /// </summary>
    private void CloseRadialMenu()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        _radialMenuIsSetActive = false;
        _radialMenu.SetActive(_radialMenuIsSetActive);

        StartCoroutine(MarkerDuration(_marker));
        Time.timeScale = StandardTimeFactor;

        // Restore saved camera input
        CinemachineCore.GetInputAxis = _cameraInput;
    }

    /// <summary>
    /// ShowMarker is present in the abstract class AbstractPingController.
    /// This is why it is protected.
    /// It overrides the method that is present in that class.
    /// A marker gets instantiated on the pinged position.
    /// </summary>
    /// <param name="position">The position the player pings.</param>
    protected override void ShowMarker(Vector3 position)
    {
        if (_marker) Destroy(_marker);
        
        switch (_pingAction)
        {
            case PingType.Hide:
                _marker = Instantiate(_markerPrefabHide, position, Quaternion.identity);
                break;
            case PingType.Interact:
                _marker = Instantiate(_markerPrefabInteract, position, Quaternion.identity);
                break;
            case PingType.Move:
                _marker = Instantiate(_markerPrefab, position, Quaternion.identity);
                break;
        }
    }

    /// <summary>
    /// Calculates the size of segments.
    /// </summary>
    /// <param name="angle">The amount of degrees of the radial menu.</param>
    /// <returns></returns>
    private static float SetDegreesFull(float angle)
    {
        if (!(angle < 0)) return angle;
        angle += DegreesFull;
        return angle;
    }

    /// <summary>
    /// Checks if the mouse is hovering over the middle segment.
    /// </summary>
    /// <returns>Returns if hovered on middle segment.</returns>
    private bool CheckInMiddleSegment()
    {
        return _inputMouse.x is < SizeCircle and > -SizeCircle && _inputMouse.y is < SizeCircle and > -SizeCircle;
    }

    /// <summary>
    /// Determines if the middle segment was selected.
    /// </summary>
    private void DetermineValueCancelled()
    {
        _cancelled = CheckInMiddleSegment()
            ? "Cancelled"
            : NotCancelled;
    }

    /// <summary>
    /// Getter for the _canceled variable.
    /// </summary>
    /// <returns>If the middle segment was selected.</returns>
    private string GetCancel()
    {
        return _cancelled;
    }
}