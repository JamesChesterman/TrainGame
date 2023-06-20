using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBlock : MonoBehaviour
{   
    public GameController gameController;
    public GameObject track;
    public GameObject cornerTrack;
    public Transform trackPos;
    private bool hasTrack;

    private int iInLevelArray;
    private int jInLevelArray;
    public GameObject[] tracksAvailable;
    public string[] directions = {"Horizontal", "Vertical", "RightDown", "LeftDown", "RightUp", "LeftUp"};  //These will match with the gameObjects to place
    private GameObject newTrack;

    public void placeTrack(){
        if(hasTrack == false){
            gameController.setTrackPlaced(iInLevelArray, jInLevelArray);
            string[,] tracksToPlace = gameController.getTrackDirections();
            string trackToPlace = tracksToPlace[iInLevelArray, jInLevelArray];
            int indexToUse = -1;
            for(int i=0; i<directions.Length; i++){
                if(directions[i] == trackToPlace){
                    indexToUse = i;
                }
            }
            newTrack = Instantiate(tracksAvailable[indexToUse], trackPos.position, tracksAvailable[indexToUse].transform.rotation);
            hasTrack = true;
        }   
    }

    public void redoTrack(){
        if(newTrack != null){
            Destroy(newTrack.gameObject);
            string[,] tracksToPlace = gameController.getTrackDirections();
            string trackToPlace = tracksToPlace[iInLevelArray, jInLevelArray];
            int indexToUse = -1;
            for(int i=0; i<directions.Length; i++){
                if(directions[i] == trackToPlace){
                    indexToUse = i;
                }
            }
            newTrack = Instantiate(tracksAvailable[indexToUse], trackPos.position, tracksAvailable[indexToUse].transform.rotation);
        }
    }

    //Keeps track of where this object is in the array of level blocks
    public void setIandJ(GameController gameControllerPassed, int i, int j, GameObject[] trackObjs){
        gameController = gameControllerPassed;
        iInLevelArray = i;
        jInLevelArray = j;
        tracksAvailable = trackObjs;
    }

    // Start is called before the first frame update
    void Start()
    {
        hasTrack = false;
    }

}
