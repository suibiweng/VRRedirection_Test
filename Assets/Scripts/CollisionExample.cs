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
        print("in start");
        spot.Task(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool TouchOne=false;

    void OnCollisionEnter(Collision other) {
        print("Touch");
        if(other.gameObject.tag =="rightHand" || other.gameObject.tag =="lefttHand" ){
            if(!TouchOne)
                {
                    TouchOne=true;
                    print("hand dog collision");
                    spot.Task(1);
                }

        }
        
        
    }

     void OnCollisionExit(Collision other) {
        print("Touch2");
        if(other.gameObject.tag =="rightHand" || other.gameObject.tag =="lefttHand" ){
        TouchOne=false;
        }
        
    }


    
}
