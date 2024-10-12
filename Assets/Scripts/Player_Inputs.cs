using UnityEngine;
using UnityEngine.InputSystem;

public class player_inputs : MonoBehaviour
{
    [SerializeField] PlayerInput InputAction;

    public bool isInteracting = false;
    public bool isShooting = false;
    public bool menu = false;
    public Vector2 movement = Vector2.zero;

    public void Movement(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isInteracting = true;
        }
        else if (context.canceled)
        {
            isInteracting = false;
        }
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isShooting = true;
        }
        else if (context.canceled)
        {
            isShooting = false;
        }
    }

    public void Menu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            menu = true;
        }
        else if (context.canceled)
        {
            menu = false;
        }
    }
}
