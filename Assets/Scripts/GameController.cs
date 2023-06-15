using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    //This will be an arraylist of arraylists (routes)
    //Each subarraylist (route) will consist of arrays of length 2 (coordinates)
    private List<List<int[]>> routeList2D = new List<List<int[]>>();

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

    //private void checkIfTrackStartOrEndOfRoute(){

    //}

    private void checkIfNewRoute(int i, int j){
        //If it's next to a track (not including diagonals) AND this track is the end of a route 
        //Then the newly placed track becomes part of the same route
        //Look up
        //Need to check that i isn't zero
        if(tracks[i-1, j] == 1){
            for(int p=0; p<routeList2D.Count; p++){
                //Check each route to see if these coordinates are at the start or end of each route
                int[] startOfRoute = routeList2D[p][0];
                int[] endOfRoute = routeList2D[p][routeList2D[p].Count-1];
                if(startOfRoute[0] == (i-1) && startOfRoute[1] == j){
                    //Add new track to start of route
                    routeList2D[p].Insert(0, new int[] {i,j});
                    //Need to check that there aren't any other tracks that the track is also connected to
                    //So if the new track is between two pieces of track, it needs to join the 2 together

                    return;
                }else if(endOfRoute[0] == (i-1) && endOfRoute[1] == j){
                    //Add new track to end of route
                    routeList2D[p].Add(new int[] {i, j});
                    //Need to check that there aren't any other tracks that the track is also connected to
                    //So if the new track is between two pieces of track, it needs to join the 2 together

                    return;
                }
            }
        }

        //Checked all directions, there's nothing surrounding the new track that is a start / end point of a route
        //Make new route
        routeList2D.Add(new List<int[]> ());
        routeList2D[routeList2D.Count-1].Add(new int[] {i,j});
    }

    public void setTrackPlaced(int i, int j){
        tracks[i,j] = 1;
        //Print the array
        printArray(tracks);
        checkIfNewRoute(i,j);
        string dynamicArrayAsString = string.Join(", ",
            routeList2D.Select(subarray =>
                $"[{string.Join(", ", subarray.Select(arr => $"[{string.Join(", ", arr)}]"))}]"
            ));
        Debug.Log(dynamicArrayAsString);
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
