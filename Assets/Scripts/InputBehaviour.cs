using UnityEngine;
using UnityEngine.InputSystem;

public class InputBehaviour : MonoBehaviour
{
    // Singleton.
    [HideInInspector]
    public static InputBehaviour Instance;

    // Readonly.
    public Vector2 OnMoveVector => _onMoveVector;
    public Vector2 OnLookVector => _onLookVector;

    private Vector2 _onMoveVector;
    private Vector2 _onLookVector;

    // Events.
    public delegate void InputBehaviourEvent(InputAction.CallbackContext ctx);
    public event InputBehaviourEvent OnMove;
    public event InputBehaviourEvent OnThrow;
    public event InputBehaviourEvent OnPingMenu;
    public event InputBehaviourEvent OnPingQuick;
    public event InputBehaviourEvent OnCallBrother;
    public event InputBehaviourEvent OnItemInteract;
    public event InputBehaviourEvent OnToggleStealth;
    public event InputBehaviourEvent OnObstacleInteract;
    public event InputBehaviourEvent OnToggleDebugginTools;

    // Private.
    private InputManager _inputs;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("Destroy");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Create");
            Instance = this;
            DontDestroyOnLoad(gameObject);
        };

        _inputs = new();

        HandleInputActions();
    }

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

    private void OnEnable()
    {
        _inputs.Enable();
    }

    private void OnDisable()
    {
        _inputs.Disable();
    }
}
