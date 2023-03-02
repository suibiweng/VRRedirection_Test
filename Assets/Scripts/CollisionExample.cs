using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionExample : MonoBehaviour
{
    public CmdCallSpot spot;
    // Start is called before the first frame update
    void Start()
    {
        spot = GameObject.FindObjectOfType<CmdCallSpot>().GetComponent<CmdCallSpot>();
        spot.StartBat();
        spot.Task(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool TouchOne=false;

    void OnCollisionEnter(Collision other) {
        print("Touch");

        if(!TouchOne)
    {
        TouchOne=true;

        spot.Task(1);
    }
       
        
        
    }

     void OnCollisionExit(Collision other) {

        TouchOne=false;
        
    }


    
}
