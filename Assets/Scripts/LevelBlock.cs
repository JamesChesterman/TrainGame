using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBlock : MonoBehaviour
{   
    public GameController gameController;
    public GameObject track;
    public Transform trackPos;
    private bool hasTrack;

    private int iInLevelArray;
    private int jInLevelArray;

    public void placeTrack(){
        if(hasTrack == false){
            GameObject newTrack = Instantiate(track, trackPos.position, trackPos.rotation) as GameObject;
            hasTrack = true;
            gameController.setTrackPlaced(iInLevelArray, jInLevelArray);
        }   
    }

    //Keeps track of where this object is in the array of level blocks
    public void setIandJ(GameController gameControllerPassed, int i, int j){
        gameController = gameControllerPassed;
        iInLevelArray = i;
        jInLevelArray = j;
    }

    // Start is called before the first frame update
    void Start()
    {
        hasTrack = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
