using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBlock : MonoBehaviour
{
    public GameObject track;
    public Transform trackPos;
    private bool hasTrack;

    public void placeTrack(){
        if(hasTrack == false){
            GameObject newTrack = Instantiate(track, trackPos.position, trackPos.rotation) as GameObject;
        }   
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
