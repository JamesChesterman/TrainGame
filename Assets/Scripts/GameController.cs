using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int levelWidth = 5;
    public int levelHeight = 5;
    public GameObject levelBlock;
    public Transform startPoint;
    private Transform currentPoint;
    public Vector3 differencePerColumn = new Vector3(3.2f, 0f, 0f);
    public Vector3 differencePerRow = new Vector3(0f, 0f, -3.2f);
    
    private int[,] tracks;

    private void initialiseArrays(){
        GameObject[,] levelBlockArray = new GameObject[levelHeight, levelWidth];
        tracks = new int[levelHeight, levelWidth];

        currentPoint = startPoint;
        for(int i=0; i<levelHeight; i++){
            for(int j=0;j<levelWidth; j++){

                GameObject newBlock = Instantiate(levelBlock, currentPoint.position, currentPoint.rotation);
                newBlock.GetComponent<LevelBlock>().setIandJ(this, i, j);
                levelBlockArray[i,j] = newBlock;

                tracks[i,j] = 0;
                currentPoint.Translate(differencePerColumn);
                
            }
            //Want to go back to the start of the row then down one.
            currentPoint.Translate(differencePerColumn * -levelWidth);
            startPoint.Translate(differencePerRow);
        }
    }

    public void setTrackPlaced(int i, int j){
        tracks[i,j] = 1;
        //Print the array
        printArray(tracks);
    }

    public void printArray(int[,] array2D){
        string arrayToPrint = "";
        for(int i=0; i<array2D.GetLength(0); i++){
            for(int j=0; j<array2D.GetLength(1); j++){
                arrayToPrint += array2D[i,j].ToString();
            }
            arrayToPrint += "\n";
        }
        Debug.Log(arrayToPrint);
    }

    // Start is called before the first frame update
    void Start()
    {   
        initialiseArrays();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
