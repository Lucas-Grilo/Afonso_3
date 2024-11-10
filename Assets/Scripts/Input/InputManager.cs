using System;
using UnityEngine.InputSystem;

public class InputManager
{
    private static InputManager _instance;

    // Propriedade para acessar a instância única
    public static InputManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new InputManager(); // Cria a instância se não existir
            }
            return _instance;
        }
    }

    private PlayerControls playerControls;

    public float Movement => playerControls.Gameplay.Movement.ReadValue<float>();

    public event Action OnJump;
    public event Action OnAttack;
    public event Action OnSlide;
    public event Action<bool> OnCrouch;

    private InputManager()
    {
        playerControls = new PlayerControls();
        EnablePlayerInput();

        playerControls.Gameplay.Jump.performed += OnJumpPerformed;
        playerControls.Gameplay.Attack.performed += OnAttackPerformed;
        playerControls.Gameplay.Slide.performed += OnSlidePerformed;
        playerControls.Gameplay.Crouch.started += OnCrouchStarted;
        playerControls.Gameplay.Crouch.canceled += OnCrouchCanceled;
    }

    private void OnJumpPerformed(InputAction.CallbackContext context) => OnJump?.Invoke();
    private void OnAttackPerformed(InputAction.CallbackContext context) => OnAttack?.Invoke();
    private void OnSlidePerformed(InputAction.CallbackContext context) => OnSlide?.Invoke();
    private void OnCrouchStarted(InputAction.CallbackContext context) => OnCrouch?.Invoke(true);
    private void OnCrouchCanceled(InputAction.CallbackContext context) => OnCrouch?.Invoke(false);

    public void DisablePlayerInput() => playerControls.Gameplay.Disable();
    public void EnablePlayerInput() => playerControls.Gameplay.Enable();

    public void Cleanup()
    {
        playerControls.Gameplay.Jump.performed -= OnJumpPerformed;
        playerControls.Gameplay.Attack.performed -= OnAttackPerformed;
        playerControls.Gameplay.Slide.performed -= OnSlidePerformed;
        playerControls.Gameplay.Crouch.started -= OnCrouchStarted;
        playerControls.Gameplay.Crouch.canceled -= OnCrouchCanceled;

        playerControls.Gameplay.Disable();
        playerControls = null;
    }

    ~InputManager() => Cleanup();
}
