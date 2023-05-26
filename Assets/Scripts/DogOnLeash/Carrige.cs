using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrige : MonoBehaviour
{
    // Start is called before the first frame update



    public Transform head;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        transform.position=new Vector3(head.position.x,this.transform.position.y,head.position.z);
        
    }
}
