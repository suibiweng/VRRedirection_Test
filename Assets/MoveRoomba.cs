using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class MoveRoomba : MonoBehaviour
{
    IPEndPoint remoteEndPoint;
    UdpClient client;
    public GameObject robot;
    public GameObject target;
    private Vector3 r_pos;
    private Vector3 t_pos;
    private float x_rot;
    private float y_rot;

    public class R_Data
    {
        public float angle;
        public float dist;

        public R_Data(float a, float d)
        {
            angle = a;
            dist = d;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    	// IP and port
    	remoteEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.102"), 8585);
    	client = new UdpClient();
        robot = GameObject.Find("Roomba");
        target = GameObject.Find("Target");
        Debug.Log("target-"+target.transform.position);
    }

    // Update is called once per frame
    void Update()
    {   robot = GameObject.Find("Roomba");
    	r_pos = robot.transform.position;
        target = GameObject.Find("Target");
        
        //Debug.Log("target in upd-"+target.transform.position);
        //target.transform.position = new Vector3(10,10,0);
         r_pos = robot.transform.position;
         t_pos = target.transform.position;
         if(Vector3.Distance(r_pos,t_pos)<0.5f){
            sendString("stop");
         }
    	if(Input.GetMouseButtonDown(0)){
    	robot = GameObject.Find("Roomba");
    	r_pos = robot.transform.position;
        target = GameObject.Find("Target");
        
        Debug.Log("target in upd-"+target.transform.position);
        //target.transform.position = new Vector3(10,10,0);
         r_pos = robot.transform.position;
         t_pos = target.transform.position;
         
         //x_rot = robot.transform.rotation.x - target.transform.rotation.x;
         //y_rot = robot.transform.rotation.y - target.transform.rotation.y;


    	double dx = r_pos.x - t_pos.x;
    	double dy = r_pos.z - t_pos.z;
    	double dist = Math.Sqrt(dx*dx + dy*dy);
    	Debug.Log("x_rot"+ x_rot+y_rot);
    	double rot_rad = Math.Atan2(dy,  dx);
    	double rot_deg = (180/Math.PI)*rot_rad;
    	//float step = 5 * Time.deltaTime;
        //robot.transform.position = Vector3.MoveTowards(transform.position, t_pos, step);
    	//Debug.Log("rot_deg"+rot_deg);
    	float r = (float) rot_deg;
    	float d = (float) dist;


    	robot.transform.eulerAngles = new Vector3(r,0,0);
    	robot.transform.position = new Vector3(d,0,0);
        string angle = rot_deg.ToString();
        string dt = d.ToString();
        //var data = new Dictionary<string, string>(){{}}
        string a = "angle:"+angle;
        string ds = "dist:"+dt;
        string dataToSend = a+","+ds;
        //sendString(dataToSend+"\n");
        Debug.Log("dataToSend-"+dataToSend);
    	//sendString(a+"\n");
        float ri=r;
        if(r<0){
            ri=180+r;
        }


        R_Data myObject = new R_Data(r,d);
        // myObject.angle = r;
        // myObject.dist = d;
        Debug.Log("myObject-"+myObject.angle +','+ myObject.dist);
        string jsonStringTrial = JsonUtility.ToJson(myObject);
        sendString(jsonStringTrial);

    	
    }
    }
    public void sendString(string message)
   {
     byte[] stringList = Encoding.UTF8.GetBytes(message);
    client.Send(stringList, stringList.Length, remoteEndPoint);
    print("message " + message + " sent to " + remoteEndPoint);
   }
    
        
 
}
