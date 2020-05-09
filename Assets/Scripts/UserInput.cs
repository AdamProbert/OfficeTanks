using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankController))]
public class UserInput : MonoBehaviour
{
    [SerializeField] bool debugMode;
    Camera cam;
    TankController tankController;
    InputMaster controls; // Our custom InputSystem control scheme
    Vector2 moveInput;
    Vector2 mousePosition;
    private void Awake() 
    {
        controls = new InputMaster();
        tankController = GetComponent<TankController>();    
        cam = Camera.main;
    }

    private void FixedUpdate() 
    {
        MovemementHandler();
        AimHandler();
        FireHandler();
    }

    private void MovemementHandler()
    {
        moveInput = controls.Player.Move.ReadValue<Vector2>();
        if(moveInput != Vector2.zero)
        {
            tankController.Move(moveInput.x, moveInput.y);    
        }
    }

    private void AimHandler()
    {   
        mousePosition = controls.Player.Look.ReadValue<Vector2>();
        Ray ray = cam.ScreenPointToRay(mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 50, Color.yellow);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 50))
        {
            tankController.Aim(hit.point.x, hit.point.z);
        }
        
    }

    private void FireHandler()
    {
        if(controls.Player.Fire.triggered == true)
        {
            tankController.Shoot();
        }
    }

    private void OnEnable() 
    {
        controls.Player.Enable();    
    }

    private void OnDisable() 
    {
        controls.Player.Disable();    
    }
}
