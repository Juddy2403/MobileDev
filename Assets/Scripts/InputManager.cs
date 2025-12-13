using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    private InputActions _inputActions;
    private Camera _camera;
    public Vector2 WorldPosition { get; private set; }
    public event System.Action PointerDown;
    public event System.Action PointerUp;
    public event System.Action<Vector2> PointerMove;

    protected override void Awake()
    {
        base.Awake();
        _inputActions = new InputActions();
    }

    private void Start()
    {
        _camera = Camera.main;
    }

    void OnEnable()
    {
        _inputActions.Enable();
        _inputActions.Player.Touch.started += OnDown;
        _inputActions.Player.Touch.canceled += OnUp;
    }

    void OnDisable()
    {
        _inputActions.Player.Touch.started -= OnDown;
        _inputActions.Player.Touch.canceled -= OnUp;
        _inputActions.Disable();
    }

    void OnDown(InputAction.CallbackContext ctx)
    {
        UpdateWorldPos();
        PointerDown?.Invoke();
        // see if there's any 2d collider under the touch
        RaycastHit2D hit = Physics2D.Raycast(WorldPosition, Vector2.zero);
        if (hit.collider == null) return;
        var interactable = hit.collider.gameObject.GetComponent<IInteractable>();
        interactable?.OnTouchStart();
    }

    private void Update()
    {
        UpdateWorldPos();
        PointerMove?.Invoke(WorldPosition);
    }

    private void UpdateWorldPos()
    {
        Vector3 screen = _inputActions.Player.TouchMove.ReadValue<Vector2>();
        WorldPosition = _camera.ScreenToWorldPoint(screen);
    }

    void OnUp(InputAction.CallbackContext ctx)
    {
        PointerUp?.Invoke();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(WorldPosition, 0.1f);
    }
}
