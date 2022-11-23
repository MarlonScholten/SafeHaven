using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Author: Hugo Verweij <br/>
/// Modified by: - <br/>
/// <para>Input behaviour script. It catches every single native input in one script, and allows other scripts to pull from the data it makes available.</para>
/// <para>For input actions that only use a value or pass through, readonly variables are used. (See <see cref="OnMoveVector"/>)</para>
/// <para>For input actions that are single use, or rely on more complex behaviour, events are used. (See <see cref="OnToggleDebugginTools"/>)</para>
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
    /// <param name="ctx">The default callback context the new input system returns.</param>
    public delegate void InputBehaviourEvent(InputAction.CallbackContext ctx);

    /// <summary>
    /// 'Throw towards pointer when throwable item in hand.'
    /// Uses F as the action key.
    /// </summary>
    public event InputBehaviourEvent OnThrow;

    /// <summary>
    /// 'Opens radial menu.'
    /// Uses RMB(LMB for selection actions) as the action key.
    /// </summary>
    public event InputBehaviourEvent OnPingMenu;

    /// <summary>
    /// 'Hides brother at "cursor to world" location. If that location is not an a hiding spot, he will choose the nearest one to him.'
    /// Uses LMB as the action key.
    /// </summary>
    public event InputBehaviourEvent OnPingQuick;

    /// <summary>
    /// 'Calls brother to return to the player character.'
    /// Uses Q as the action key.
    /// </summary>
    public event InputBehaviourEvent OnCallBrother;

    /// <summary>
    /// 'Player needs to look at target:'
    /// <para>Empty hand = pick up.</para>
    /// <para>Item in hand = drop item.</para>
    /// <para>Item in hand + looking at new item = swap item.</para>
    /// <para>Item in hand + looking at interaction point = use item.</para>
    /// Uses E as the action key.
    /// </summary>
    public event InputBehaviourEvent OnItemInteract;

    /// <summary>
    /// 'Enter & Exit stealth mode.'
    /// Uses (LEFT) Control as the action key.
    /// </summary>
    public event InputBehaviourEvent OnToggleStealth;

    /// <summary>
    /// 'Gets past an obstacle.'
    /// Uses Space as the action key.
    /// </summary>
    public event InputBehaviourEvent OnObstacleInteract;

    /// <summary>
    /// 'Show & Hide the debugger.'
    /// Uses F12 as the action key.
    /// </summary>
    public event InputBehaviourEvent OnToggleDebugginTools;

    // Private.
    private InputManager _inputs;

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

        _inputs = new();

        HandleInputActions();
    }

    /// <summary>
    /// <para>Subscribe to the actual input events, and channel it through to the external events.</para>
    /// <para>1. Create input action through the unity editor.</para>
    /// <para>2. Create <see cref="InputBehaviourEvent"/> that external scripts can use.</para>
    /// <para>2. Create <see cref="OnMoveVector"/> like vector2 that external scripts can use.</para>
    /// <para>3. Subscribe to the native performed, and invoke the custom event that has just been created.</para>
    /// </summary>
    private void HandleInputActions()
    {
        // Player.
        _inputs.Player.Throw.performed += (ctx) => OnThrow?.Invoke(ctx);
        _inputs.Player.PingMenu.performed += (ctx) => OnPingMenu?.Invoke(ctx);
        _inputs.Player.PingQuick.performed += (ctx) => OnPingQuick?.Invoke(ctx);
        _inputs.Player.CallBrother.performed += (ctx) => OnCallBrother?.Invoke(ctx);
        _inputs.Player.ItemInteract.performed += (ctx) => OnItemInteract?.Invoke(ctx);
        _inputs.Player.ToggleStealth.performed += (ctx) => OnToggleStealth?.Invoke(ctx);
        _inputs.Player.ObstacleInteract.performed += (ctx) => OnObstacleInteract?.Invoke(ctx);

        // UI.
        _inputs.UI.ToggleDebugginTools.performed += (ctx) => OnToggleDebugginTools?.Invoke(ctx);

        // Custom logic.
        _inputs.Player.Move.performed += (ctx) => _onMoveVector = ctx.ReadValue<Vector2>();
        _inputs.Player.Move.canceled += (ctx) => _onMoveVector = ctx.ReadValue<Vector2>();
        _inputs.Player.Look.performed += (ctx) => _onLookVector = ctx.ReadValue<Vector2>();
        _inputs.Player.Look.canceled += (ctx) => _onLookVector = ctx.ReadValue<Vector2>();
    }

    /// <summary>
    /// Enables the <see cref="InputManager"/> whenever <see cref="InputBehaviour"/> is active.
    /// </summary>
    private void OnEnable()
    {
        _inputs.Enable();
    }

    /// <summary>
    /// Enables the <see cref="InputManager"/> whenever <see cref="InputBehaviour"/> is inactive.
    /// </summary>
    private void OnDisable()
    {
        _inputs.Disable();
    }
}