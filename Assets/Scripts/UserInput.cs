using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(TankController))]
public class UserInput : MonoBehaviour
{
    [SerializeField] bool debugMode;

    PlayerInput playerInput;
    Camera cam;
    TankController tankController;
    Vector2 moveInput = Vector2.zero;
    Vector2 mousePosition = Vector2.zero;
    string currentControlScheme = "";

    private void Awake() 
    {
        tankController = GetComponent<TankController>();    
        playerInput = GetComponent<PlayerInput>();
        currentControlScheme = playerInput.currentControlScheme;
        cam = Camera.main;
    }

    private void FixedUpdate() {
        // Movement
        if(moveInput != Vector2.zero)
        {
            tankController.Move(moveInput.x, moveInput.y);
        }

        // Aim
        if(mousePosition != Vector2.zero)
        {
            if(currentControlScheme == "Keyboard&Mouse")
            {
                Ray ray = cam.ScreenPointToRay(mousePosition);
                if(debugMode)
                {
                    Debug.DrawRay(ray.origin, ray.direction * 50, Color.yellow);
                }
                
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit, 50))
                {
                    tankController.AimAtPoint(hit.point.x, hit.point.z);
                }
            }
            else if (currentControlScheme == "Gamepad")
            {
                if(mousePosition != Vector2.zero)
                {
                    if(debugMode)
                    {
                        Debug.DrawRay(transform.position, new Vector3(mousePosition.x, 0, mousePosition.y) * 50, Color.yellow);
                    }
                    tankController.AimAtDirection(mousePosition.x, mousePosition.y);
                }
            }
            else
            {
                Debug.LogWarning("Unknown control scheme, unable to handle.");
            }
        }
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        if(debugMode) Debug.Log("MoveInput: " + moveInput);
    }

    public void OnLook(InputValue value)
    {
        mousePosition = value.Get<Vector2>();
        if(debugMode) Debug.Log("MousePosition: " + mousePosition);
    }

    public void OnControlsChanged()
    {
        if(debugMode) Debug.Log("Controls changed to: " + playerInput.currentControlScheme);
        currentControlScheme = playerInput.currentControlScheme;
    }

    public void OnFire()
    {
        tankController.Shoot();
    }
}
