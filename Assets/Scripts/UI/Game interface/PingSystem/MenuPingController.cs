using System;
using UnityEngine;
using UnityEngine.InputSystem;
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
    [SerializeField] private GameObject _highlightedOption;

    /// <summary>
    /// When the radial menu is open time slows down in game, this factor provides the ability to tweak how much time slows down.
    /// </summary>
    [SerializeField] private int _slowmotionFactor = 4;

    /// <summary>
    /// Different parts of the radial menu have varying colors.
    /// The radialMenuNormal controls the color of the text in the segments of the radial menu when nothing hovers over it.
    /// The radialMenuOptionHovered controls the color of the text in the outside segment of the radial menu when the mouse hovers over it.
    /// The radialMenuCancel controls the color of the text in the middle segment of the radial menu when the mouse hovers over it.
    /// </summary>
    [SerializeField] private Color radialMenuNormal, radialMenuOptionHovered, radialMenuCancel;

    /// <summary>
    /// In the radial menu are different sections.
    /// These are the different Text objects that are present in the radial menu.
    /// </summary>
    [SerializeField] private Text[] options;

    /// <summary>
    /// In the middle of the radial menu is a cancel functionality.
    /// In here is the Text object from the cancel option.
    /// </summary>
    [SerializeField] private Text cancel;

    private Vector2 _inputMouse;
    private int _selectedOption;
    private GameObject _marker;
    private bool _radialMenuIsSetActive = false;
    private PingType _pingAction;
    private PingType _chosenAction;
    private float _degreesPerSegment;
    private string _cancelled;

    private const int StandardTimeFactor = 1;
    private const int Two = 2;
    private const float StartingPointCorrection = 90f;
    private const float DegreesHalf = 180f;
    private const float DegreesFull = 360f;
    private const string NotCancelled = "Not cancelled";
    private const float SizeCircle = 45f;

    /// <summary>
    /// In order to let the input actions work Awake needs to be protected.
    /// </summary>
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

    /// <summary>
    /// When the player presses the left mouse button an action can be selected or the radial menu can be cancelled.
    /// </summary>
    /// <param name="callbackContext">Information provided to action callbacks about what triggered an action. Needed to make the function react on actions.></param>
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

    /// <summary>
    /// When the player presses the right mouse button a ping gets placed, time slows down and the radial menu gets activated.
    /// </summary>
    /// <param name="callbackContext">Information provided to action callbacks about what triggered an action. Needed to make the function react on actions.></param>
    private void OnMenuPing(InputAction.CallbackContext callbackContext)
    {
        if (_radialMenuIsSetActive) return;
        _radialMenuIsSetActive = true;

        var ray = GetRayFromCameraToMousePosition();
        SetPingPosition(ray);
        ShowMarker(_pingPosition);

        Time.timeScale /= _slowmotionFactor;
    }

    /// <summary>
    /// The chosen action from the player gets set to the ping action.
    /// The ping action together with the ping position will be passed on to the Brother AI.
    /// This is the place where the integration with the Brother AI takes place.
    /// </summary>
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

    /// <summary>
    /// ShowMarker is present in the abstract class AbstractPingController.
    /// This is why it is protected.
    /// It overrides the method that is present in that class.
    /// A marker gets instantiated on the pinged position.
    /// </summary>
    /// <param name="position">The position the player pings.</param>
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

    private string GetCancel()
    {
        return _cancelled;
    }
}