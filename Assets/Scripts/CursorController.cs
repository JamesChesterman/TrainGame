using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{

    public Texture2D cursor;
    public Texture2D cursorClicked;

    private CursorControls controls;
    private Camera mainCamera;

    //Changes the cursor image depending on whether you have clicked or not
    private void changeCursor(Texture2D cursorType){
        Cursor.SetCursor(cursorType, Vector2.zero, CursorMode.Auto);
    }

    private void OnEnable(){
        controls.Enable();
    }

    private void onDisable(){
        controls.Disable();
    }

    private void Awake(){
        controls = new CursorControls();
        changeCursor(cursor);
        Cursor.lockState = CursorLockMode.Confined;
        mainCamera = Camera.main;
    }

    private void startedClick(){
        changeCursor(cursorClicked);
    }

    private void endedClick(){
        changeCursor(cursor);
        detectObj();
    }

    private void detectObj(){
        //Use raycast to see if we're clicking anything in the scene
        Ray ray = mainCamera.ScreenPointToRay(controls.Mouse.Position.ReadValue<Vector2>());
        RaycastHit hit;
        //This might need to be improved at a later date. Come here if the clicking isn't really working properly. (Involves LayerMasks)
        if(Physics.Raycast(ray, out hit)){
            if(hit.collider != null){
                if(hit.collider.tag == "LevelBlock"){
                    hit.collider.gameObject.GetComponent<LevelBlock>().placeTrack();
                }
            }
        } 
    }
    
    // Start is called before the first frame update
    private void Start()
    {   
        //Uses new Unity input system.
        //These 2 are lambda functions that map the mouse event to the right function
        controls.Mouse.Click.started += _ =>  startedClick();
        controls.Mouse.Click.performed += _ => endedClick();
    }

}
