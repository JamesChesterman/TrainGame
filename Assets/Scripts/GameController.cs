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

    //The new track has been placed between two separate routes. So need to join them together
    //private void mergeTwoRoutes(int route1, int route2){

    //}

    //In checkIfNewRoute, you look up, down, left and right
    //If there's a track there, you pass it to this function
    //This function then determines if this piece of track is at the start or the end of the route
    //If so, then you need to add the new track to the start / end of the route
    private int checkIfTrackStartOrEndOfRoute(int trackX, int trackY, int newTrackX, int newTrackY, string direction){
        for(int p=0; p<routeList2D.Count; p++){
            //Check each route to see if these coordinates are at the start or end of each route
            int[] startOfRoute = routeList2D[p][0];
            int[] endOfRoute = routeList2D[p][routeList2D[p].Count-1];
            if(startOfRoute[0] == trackX && startOfRoute[1] == trackY){
                //Add new track to start of route
                routeList2D[p].Insert(0, new int[] {newTrackX, newTrackY});
                return p;
            }else if(endOfRoute[0] == trackX && endOfRoute[1] == trackY){
                //Add new track to end of route
                routeList2D[p].Add(new int[] {newTrackX, newTrackY});
                return p;
            }
        }
        return -1;
    }

    //If it's next to a track (not including diagonals) AND this track is the end of a route 
    //Then the newly placed track becomes part of the same route
    private void checkIfNewRoute(int i, int j){
        List<int> routesTrackAddedTo = new List<int>();
        //Look up
        //Need to check that i isn't zero
        if(i != 0){
            if(tracks[i-1,j] == 1){
                routesTrackAddedTo.Add(checkIfTrackStartOrEndOfRoute(i-1, j, i, j, "up"));
            }
        }
        //Look down
        if(i != tracks.Length-1){
            if(tracks[i+1,j] == 1){
                routesTrackAddedTo.Add(checkIfTrackStartOrEndOfRoute(i+1, j, i, j, "down"));
            }
        }
        //Look left
        if(j != 0){
            if(tracks[i,j-1] == 1){
                routesTrackAddedTo.Add(checkIfTrackStartOrEndOfRoute(i, j-1, i, j, "left"));
            }
        }
        //Look right
        if(j != tracks.GetLength(1)-1){
            if(tracks[i,j+1] == 1){
                routesTrackAddedTo.Add(checkIfTrackStartOrEndOfRoute(i, j+1, i, j, "right"));
            }
        }
        
        int routeFound = -1;
        for(int x=0;x<routesTrackAddedTo.Count;x++){
            if(routesTrackAddedTo[x] != -1 && routeFound == -1){
                routeFound = routesTrackAddedTo[x];
                continue;
            }
            if(routesTrackAddedTo[x] != -1 && routeFound != -1){
                //Already found a route. So need to join these 2 routes together
                //If the two tracks are part of the same route, then you need to make this new track as part of a separate route
                //Otherwise you can place tracks in a circle
                break;
            }
        }
        if(routeFound == -1){
            //Make new route
            routeList2D.Add(new List<int[]> ());
            routeList2D[routeList2D.Count-1].Add(new int[] {i,j});
        }
    }

    public void setTrackPlaced(int i, int j){
        tracks[i,j] = 1;
        //Print the array
        printArray(tracks);
        checkIfNewRoute(i,j);

        //Print the array of routes
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
