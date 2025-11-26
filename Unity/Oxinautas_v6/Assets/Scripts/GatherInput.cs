using UnityEngine;
using UnityEngine.InputSystem;
public class GatherInput : MonoBehaviour
{
    private Controls controls;
    [SerializeField] private float _valueX;

    public global::System.Single ValueX { get => _valueX; set => _valueX = value; }

    private void Awake()
    {
        controls = new Controls();
    }
    private void OnEnable()
    {
        controls.Player.Move.performed += StartMove;
        controls.Player.Move.canceled += StopMove;
        controls.Player.Enable();
    }
    private void StartMove(InputAction.CallbackContext context)
    {
        _valueX = context.ReadValue<float>();
    }
    private void StopMove(InputAction.CallbackContext context)
    {
        _valueX = 0;
    }
    private void OnDisable()
    {
        controls.Player.Move.performed -= StartMove;
        controls.Player.Move.canceled -= StopMove;
        controls.Disable();
    }
}
