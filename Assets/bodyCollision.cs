using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bodyCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        print("updated collision script");
        
    }
    void OnCollisionEnter(Collision other) {


        print("Touch");
        
    }
}
