using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{   
    public float acceleration = 0.01f;
    public float deceleration = 0.05f;
    public Vector2 maxVelocity = new Vector2(1, 1);
    private CameraControls cameraControls;
    private Vector2 inputVector = Vector2.zero;
    private Vector2 currentVelocity = Vector2.zero;

    private void OnMovementPerformed(InputAction.CallbackContext value){
        inputVector = value.ReadValue<Vector2>();
    }

    private void OnMovementCancelled(InputAction.CallbackContext value){
        inputVector = Vector2.zero;
    }

    private void OnEnable(){
        cameraControls.Enable();
        //Maps the InputAction created in the editor to the function 'OnMovementPerformed'
        cameraControls.Camera.Movement.performed += OnMovementPerformed;
        cameraControls.Camera.Movement.canceled += OnMovementCancelled;
    }

    private void OnDisable(){
        cameraControls.Disable();
        cameraControls.Camera.Movement.performed -= OnMovementPerformed;
        cameraControls.Camera.Movement.canceled -= OnMovementCancelled;
    }

    private void Awake(){
        cameraControls = new CameraControls();
    }

    private void FixedUpdate(){
        if(inputVector.x != 0 && currentVelocity.x <= maxVelocity.x && currentVelocity.x >= -maxVelocity.x){
            currentVelocity.x += inputVector.x * acceleration;
        }else{
            if(currentVelocity.x != 0){
                //Decelerate in the opposite direction
                float tempDeceleration = deceleration * Mathf.Sign(currentVelocity.x) * -1;
                currentVelocity.x += tempDeceleration;
                if(currentVelocity.x * tempDeceleration > 0){
                    //If the velocity is in the same direction as the deceleration, then it has overshot 0. So just set it to 0
                    currentVelocity.x = 0;
                }
            }
        }
        //Need to move relative to the world, so it doesn't matter the direction of the camera.
        transform.Translate(currentVelocity, Space.World);
    }

}
