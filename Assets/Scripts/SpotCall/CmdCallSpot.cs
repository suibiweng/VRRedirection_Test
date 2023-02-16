
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CmdCallSpot : MonoBehaviour
{
    public string folderpath;
    public string[]  batname;

    public string[] Tasks;

    // Start is called before the first frame update
    void Start()
    {
           
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartBat(){

        Process.Start(folderpath+"/"+batname[0]+".bat");


    }

        public void Task(int i){

        Process.Start(folderpath+"/"+batname[1]+".bat",Tasks[i]+".walk");


    }
}
