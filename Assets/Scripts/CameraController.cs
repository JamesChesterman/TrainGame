using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{   
    public float acceleration = 0.01f;
    public float deceleration = 0.05f;
    public Vector3 maxVelocity = new Vector3(1, 1, 1);
    private CameraControls cameraControls;
    private Vector2 inputVector = Vector2.zero;
    private float inputUpDown = 0f;
    private Vector3 currentVelocity = Vector3.zero;

    private void OnMovementPerformed(InputAction.CallbackContext value){
        inputVector = value.ReadValue<Vector2>();
    }

    private void OnMovementCancelled(InputAction.CallbackContext value){
        inputVector = Vector2.zero;
    }

    private void OnUpDownPerformed(InputAction.CallbackContext value){
        inputUpDown = value.ReadValue<float>();
    }

    private void OnUpDownCancelled(InputAction.CallbackContext value){
        inputUpDown = 0f;
    }

    private void OnEnable(){
        cameraControls.Enable();
        //Maps the InputAction created in the editor to the function 'OnMovementPerformed'
        cameraControls.Camera.Movement.performed += OnMovementPerformed;
        cameraControls.Camera.Movement.canceled += OnMovementCancelled;
        cameraControls.Camera.UpDown.performed += OnUpDownPerformed;
        cameraControls.Camera.UpDown.canceled += OnUpDownCancelled;
    }

    private void OnDisable(){
        cameraControls.Disable();
        cameraControls.Camera.Movement.performed -= OnMovementPerformed;
        cameraControls.Camera.Movement.canceled -= OnMovementCancelled;
        cameraControls.Camera.UpDown.performed -= OnUpDownPerformed;
        cameraControls.Camera.UpDown.canceled -= OnUpDownCancelled;
    }

    private void Awake(){
        cameraControls = new CameraControls();
    }

    //Need to pass by reference to modify the values that I'm referring to here
    private void move(ref float inputVectorDir, ref float currentVelocityDir, ref float maxVelocityDir){
        if(inputVectorDir != 0 && currentVelocityDir <= maxVelocityDir && currentVelocityDir >= -maxVelocityDir){
            if(currentVelocityDir * inputVectorDir < 0){
                //If the velocity is in the opposite direction to the button you're pressing, you need to add on the deceleration
                currentVelocityDir += inputVectorDir * deceleration;
            }else{
                currentVelocityDir += inputVectorDir * acceleration;
            } 
        }else{
            if(currentVelocityDir != 0){
                //Decelerate in the opposite direction
                float tempDeceleration = deceleration * Mathf.Sign(currentVelocityDir) * -1;
                currentVelocityDir += tempDeceleration;
                if(currentVelocityDir * tempDeceleration > 0){
                    //If the velocity is in the same direction as the deceleration, then it has overshot 0. So just set it to 0
                    currentVelocityDir = 0;
                }
            }
        }
    }

    private void FixedUpdate(){
        move(ref inputVector.x, ref currentVelocity.x, ref maxVelocity.x);
        move(ref inputUpDown, ref currentVelocity.y, ref maxVelocity.y);
        move(ref inputVector.y, ref currentVelocity.z, ref maxVelocity.z);
        //Need to move relative to the world, so it doesn't matter the direction of the camera.
        //Might need to bound this later to make sure it doesn't go through the floor
        transform.Translate(currentVelocity, Space.World);
    }

}
