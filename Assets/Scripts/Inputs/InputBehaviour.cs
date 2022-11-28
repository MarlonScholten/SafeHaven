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

    // Private.
    //private InputManager _inputs;

    /// <summary>
    /// Standard awake void. Sets the singleton, creates new inputs and handles the actions.
    /// </summary>
    private void Awake()
    {
        // Destroy the gameobject if it already has an instance.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        // Set the instance, and adds it to the pool of dont destroy on load.
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        };
    }

    private void OnMove(InputValue inputValue) => _onMoveVector = inputValue.Get<Vector2>();
    private void OnLook(InputValue inputValue) => _onLookVector = inputValue.Get<Vector2>();
    private void OnThrow() => OnThrowEvent?.Invoke();
    private void OnPingMenu() => OnPingMenuEvent?.Invoke();
    private void OnPingQuick() => OnPingQuickEvent?.Invoke();
    private void OnCallBrother() => OnCallBrotherEvent?.Invoke();
    private void OnItemInteract() => OnItemInteractEvent?.Invoke();
    private void OnToggleStealth() => OnToggleStealthEvent?.Invoke();
    private void OnObstacleInteract() => OnObstacleInteractEvent?.Invoke();
    private void OnToggleDebugginTools() => OnToggleDebugginToolsEvent?.Invoke();
    private void OnPause() => OnPauseEvent?.Invoke();

    /// <summary>
    /// Enables the <see cref="InputManager"/> whenever <see cref="InputBehaviour"/> is inactive.
    /// </summary>
    private void OnDisable()
    {
        _onLookVector = Vector2.zero;
        _onMoveVector = Vector2.zero;
    }
}