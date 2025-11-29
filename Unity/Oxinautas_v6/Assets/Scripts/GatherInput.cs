using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch; // Necesario para la detección de toques

public class GatherInput : MonoBehaviour
{
    private Controlls controls;

    [SerializeField] private float _valueX;
    public float ValueX { get => _valueX; }

    [SerializeField] private bool _isJumping;
    public bool IsJumping { get => _isJumping; set => _isJumping = value; } // El PlayerController debe restablecer esto a false

    // Variables públicas para definir las zonas relativas al personaje, visibles en el Inspector.
    [Header("Ajustes de Control Relativo (Píxeles)")]
    [Tooltip("Distancia mínima horizontal (en píxeles) desde el centro del personaje para activar el movimiento lateral.")]
    public float touchDeadZoneX = 100f;

    [Tooltip("Distancia mínima vertical (en píxeles) sobre la cabeza del personaje para activar el salto.")]
    public float touchJumpY = 100f;

    private void Awake()
    {
        // Inicialización del Input System y Controles
        EnhancedTouchSupport.Enable();
        controls = new Controlls();
    }

    private void Start()
    {
        // La inicialización de la pantalla ya no es necesaria, se usa WorldToScreenPoint.
    }

    private void Update()
    {
        HandleTouchInput();
    }

    // Función HandleTouchInput con soporte para Diagonal Began
    private void HandleTouchInput()
    {
        if (UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches.Count == 0)
        {
            return;
        }

        // 1. Obtener la posición del Player en la pantalla
        Camera mainCam = Camera.main;
        if (mainCam == null) return;

        Vector3 playerScreenPos = mainCam.WorldToScreenPoint(transform.position);

        // Reiniciamos el movimiento para que se detecte en el toque activo.
        _valueX = 0f;

        // 2. LÓGICA DE PULSO Y MOVIMIENTO
        foreach (var touch in UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches)
        {
            float touchX = touch.screenPosition.x;
            float touchY = touch.screenPosition.y;

            // --- JUMP LOGIC (PULSO - TouchPhase.Began) ---
            // Activa el pulso de salto si el toque comienza en la zona superior.
            if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
            {
                if (touchY > playerScreenPos.y + touchJumpY)
                {
                    _isJumping = true;
                }
            }

            // --- MOVEMENT LOGIC (CONTINUO) ---
            // El movimiento se detecta en cualquier fase donde el dedo está presionado.
            // AÑADIDO: Incluimos TouchPhase.Began para detectar la dirección al inicio de un tap (Diagonal).
            if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began ||
                touch.phase == UnityEngine.InputSystem.TouchPhase.Moved ||
                touch.phase == UnityEngine.InputSystem.TouchPhase.Stationary)
            {
                // Mover a la DERECHA
                if (touchX > playerScreenPos.x + touchDeadZoneX)
                {
                    _valueX = 1f;
                    return; // Se encontró movimiento, salimos del bucle.
                }

                // Mover a la IZQUIERDA
                else if (touchX < playerScreenPos.x - touchDeadZoneX)
                {
                    _valueX = -1f;
                    return; // Se encontró movimiento, salimos del bucle.
                }
            }
        }

        // Si llegamos aquí y no hubo movimiento, _valueX ya está en 0f.
    }

    // El resto de las funciones (OnEnable, StartMove, StopMove, etc.) permanecen igual
    // ya que manejan el input de teclado (que también es un pulso).

    private void OnEnable()
    {
        controls.Player.Move.performed += StartMove;
        controls.Player.Move.canceled += StopMove;
        controls.Player.Jump.performed += StartJump;
        controls.Player.Jump.canceled += StopJump;
        controls.Player.Enable();
    }

    private void StartMove(InputAction.CallbackContext context)
    {
        if (UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches.Count == 0)
        {
            _valueX = Mathf.RoundToInt(context.ReadValue<float>());
        }
    }
    private void StopMove(InputAction.CallbackContext context)
    {
        if (UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches.Count == 0)
        {
            _valueX = 0;
        }
    }
    private void StartJump(InputAction.CallbackContext context)
    {
        if (UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches.Count == 0)
        {
            // El teclado también envía una señal de pulso para el salto.
            _isJumping = true;
        }
    }
    private void StopJump(InputAction.CallbackContext context)
    {
        // En el input de teclado, el salto puede ser continuo si la tecla se mantiene,
        // pero lo mejor es que el PlayerController lo restablezca para mantener la consistencia.
        // Aquí no hacemos nada para el stop, el PlayerController lo manejará.
    }
    private void OnDisable()
    {
        controls.Player.Move.performed -= StartMove;
        controls.Player.Move.canceled -= StopMove;
        controls.Player.Jump.performed -= StartJump;
        controls.Player.Jump.canceled -= StopJump;
        controls.Disable();
        // Desactiva el sistema de Touch al finalizar
        EnhancedTouchSupport.Disable();
    }
}