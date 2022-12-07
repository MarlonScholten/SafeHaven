using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Author: Hugo Verweij <br/>
/// Modified by: Tom Cornelissen <br/>
/// Input behaviour script. It catches every single native input in one script, and allows other scripts to pull from the data it makes available. <br />
/// For input actions that only use a value or pass through, readonly variables are used. (See <see cref="OnMoveVector"/>) <br />
/// For input actions that are single use, or rely on more complex behaviour, events are used. (See <see cref="OnToggleDebugginToolsEvent"/>) <br />
/// </summary>
/// <list type="table">
///	    <listheader>
///         <term>On what GameObject</term>
///         <term>Type</term>
///         <term>Name of type</term>
///         <term>Description</term>
///     </listheader>
///	    <item>
///         <term>InputManager, the one that holds the player input.</term>
///		    <term>Script</term>
///         <term>InputBehaviour</term>
///		    <term>Input behaviour script. It catches every single native input in one script, and allows other scripts to pull from the data it makes available.</term>
///	    </item>
/// </list>
public class InputBehaviour : MonoBehaviour
{
    /// <summary>
    /// The static singleton modifier that can be used to access data without a direct reference.
    /// </summary>
    [HideInInspector]
    public static InputBehaviour Instance;

    // Readonly.
    /// <summary>
    /// OnMoveVector, gets updated once the player presses the movement keys.
    /// </summary>
    public Vector2 OnMoveVector => _onMoveVector;
    /// <summary>
    /// OnLookVector, gets updated once the player looks around.
    /// </summary>
    public Vector2 OnLookVector => _onLookVector;

    private Vector2 _onMoveVector;
    private Vector2 _onLookVector;

    /// <summary>
    /// Event Handler, specifies what structure the behaviour events should follow.
    /// </summary>
    public delegate void InputBehaviourEvent();

    /// <summary>
    /// 'Throw towards pointer when throwable item in hand.'
    /// Uses F as the action key.
    /// </summary>
    public event InputBehaviourEvent OnThrowEvent;

    /// <summary>
    /// 'Opens radial menu.'
    /// Uses RMB(LMB for selection actions) as the action key.
    /// </summary>
    public event InputBehaviourEvent OnPingMenuEvent;

    /// <summary>
    /// 'Hides brother at "cursor to world" location. If that location is not an a hiding spot, he will choose the nearest one to him.'
    /// Uses LMB as the action key.
    /// </summary>
    public event InputBehaviourEvent OnPingQuickEvent;
    
    /// <summary>
    /// 'Hides brother at "cursor to world" location. If that location is not an a hiding spot, he will choose the nearest one to him.'
    /// Uses LMB as the action key.
    /// </summary>
    public event InputBehaviourEvent OnPingQuickCancelledEvent;

    /// <summary>
    /// 'Calls brother to return to the player character.'
    /// Uses Q as the action key.
    /// </summary>
    public event InputBehaviourEvent OnCallBrotherEvent;

    /// <summary>
    /// 'Player needs to look at target:'
    /// <para>Empty hand = pick up.</para>
    /// <para>Item in hand = drop item.</para>
    /// <para>Item in hand + looking at new item = swap item.</para>
    /// <para>Item in hand + looking at interaction point = use item.</para>
    /// Uses E as the action key.
    /// </summary>
    public event InputBehaviourEvent OnItemInteractEvent;

    /// <summary>
    /// 'Enter & Exit stealth mode.'
    /// Uses (LEFT) Control as the action key.
    /// </summary>
    public event InputBehaviourEvent OnToggleStealthEvent;

    /// <summary>
    /// 'Gets past an obstacle.'
    /// Uses Space as the action key.
    /// </summary>
    public event InputBehaviourEvent OnObstacleInteractEvent;

    /// <summary>
    /// 'Show & Hide the debugger.'
    /// Uses F12 as the action key.
    /// </summary>
    public event InputBehaviourEvent OnToggleDebugginToolsEvent;

    /// <summary>
    /// 'Pause the game'
    /// Uses ESC or P as the action key
    /// </summary>
    public event InputBehaviourEvent OnPauseEvent;

    /// <summary>
    /// Comfort the brother.
    /// Uses C key as the action key.
    /// </summary>
    public event InputBehaviourEvent OnComfortEvent;

    // Private.
    //private InputManager _inputs;

    /// <summary>
    /// Standard awake void. Sets the singleton, creates new inputs and handles the actions.
    /// </summary>
    private void Awake()
    {
        Instance = this;
    }

    public void OnMove(InputAction.CallbackContext context) => _onMoveVector = context.ReadValue<Vector2>();
    public void OnLook(InputAction.CallbackContext context) => _onLookVector = context.ReadValue<Vector2>();
    public void OnThrow(InputAction.CallbackContext context) => OnThrowEvent?.Invoke();
    public void OnPingMenu(InputAction.CallbackContext context) => OnPingMenuEvent?.Invoke();
    public void OnComfort(InputAction.CallbackContext context) => OnComfortEvent?.Invoke();
    public void OnPingQuick(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            OnPingQuickCancelledEvent?.Invoke();
        }else if (context.performed)
        {
            OnPingQuickEvent?.Invoke();
        }
    }
    public void OnCallBrother(InputAction.CallbackContext context) => OnCallBrotherEvent?.Invoke();
    public void OnItemInteract(InputAction.CallbackContext context)
    {
        if(context.performed) OnItemInteractEvent?.Invoke();
    }
    public void OnToggleStealth(InputAction.CallbackContext context) => OnToggleStealthEvent?.Invoke();
    public void OnObstacleInteract(InputAction.CallbackContext context) => OnObstacleInteractEvent?.Invoke();
    public void OnToggleDebugginTools(InputAction.CallbackContext context) => OnToggleDebugginToolsEvent?.Invoke();
    public void OnPause(InputAction.CallbackContext context) => OnPauseEvent?.Invoke();

    /// <summary>
    /// Enables the <see cref="InputManager"/> whenever <see cref="InputBehaviour"/> is inactive.
    /// </summary>
    private void OnDisable()
    {
        _onLookVector = Vector2.zero;
        _onMoveVector = Vector2.zero;
    }
}