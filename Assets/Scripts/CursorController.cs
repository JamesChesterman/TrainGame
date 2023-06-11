using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{

    public Texture2D cursor;
    public Texture2D cursorClicked;

    private CursorControls controls;

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
    }

    private void startedClick(){
        Debug.Log("HERE");
        changeCursor(cursorClicked);
    }

    private void endedClick(){
        Debug.Log("HERE");
        changeCursor(cursor);
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
