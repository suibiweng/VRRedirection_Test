using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using System.Text;

    public class R_Data
    {
        public int Right;
        public int Left;

        public R_Data(int a, int b)
        {
            Right = a;
            Left = b;
        }
    }

    public enum RoombaStatus{
        Forward,
        Stop,
        Rotate
    }


public class TrackerRoomba : MonoBehaviour
{
    public Transform Target;
    public  bool isAlign,isNear;
   // public PsudoControl control;
    public float distance=1.5f;
    public int pwmValue = 255;
    public int pwmR=0,pwmL=0;

    UdpClient client;
    IPEndPoint remoteEndPoint;
    public string IPaddress = "192.168.0.102";
    public int Port=8585;


    RoombaStatus status;

    // Start is called before the first frame update
    void Start()
    {
        
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(IPaddress), Port);
        client = new UdpClient();


    }

      public bool EStop=false;

    // Update is called once per frame
    void Update()
    {
   if(!EStop){
    
    RoomBaMove();
   }else{

       stopRoomBa();


   }
        

        if(Input.GetKeyDown(KeyCode.F)){
            EStop=!EStop;
              stopRoomBa();
            print("Stop");
         //   StartCoroutine( MoveForward(10));
        }


        //pwmL=0;
        //pwmR=0;


        if(Input.GetKey(KeyCode.D)){
            pwmR=pwmValue;
        }


        if(Input.GetKey(KeyCode.C)){
            pwmR=-pwmValue;
        }

        
        if(Input.GetKey(KeyCode.A)){
            pwmL=pwmValue;
        }


        if(Input.GetKey(KeyCode.Z)){
            pwmL=-pwmValue;
        }


      if(Input.GetKey(KeyCode.Space)){
            pwmL=0;
            pwmR=0;
        }





     //sendDatat(pwmL, pwmR);




    }

    IEnumerator MoveForward(float DelayTime){

        sendDatat(pwmValue, pwmValue);
        yield return new WaitForSeconds(DelayTime);
        stopRoomBa();

    }


    IEnumerator MoveBackward(float DelayTime){

        sendDatat(-pwmValue, -pwmValue);
        yield return new WaitForSeconds(DelayTime);
        stopRoomBa();

    }

    void sendDatat(int R, int L) {

        R_Data myObject = new R_Data(R, L);
   
        string jsonStringTrial = JsonUtility.ToJson(myObject);
         print(jsonStringTrial);
        sendString(jsonStringTrial);
    }



    bool turnning=false;
    void RoomBaMove() {


        isNear = Vector3.Distance(Target.position, transform.position) <= distance;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        isAlign = Physics.Raycast(transform.position, fwd, Mathf.Infinity);


      
        
        
        RotateFirst();
        
    



      if(!isAlign && !isNear)turnning=true;

        if(!turnning)  MoveSecond();

    }

    void RotateFirst() {

         turnning=true;

        if (Target.position.x < transform.position.x) {
            if (!isAlign && !isNear){

                 sendDatat(-pwmValue/2, pwmValue/2);
                status=RoombaStatus.Rotate;  

            }
                   //  control.wheel(150, -150);


            if (isAlign) stopRoomBa();

        }




        if (Target.position.x > transform.position.x ) {
            if (!isAlign && !isNear ) {

                sendDatat(pwmValue/2, -pwmValue/2);
                status=RoombaStatus.Rotate; 

            }
              

            if (isAlign) stopRoomBa();

        }


       

    }



    void MoveSecond() {

        //if(status!= RoombaStatus.Stop) return; 

        if (isNear) {
            stopRoomBa();

        }
        if (isAlign && !isNear) {

            sendDatat(pwmValue, pwmValue);

     status= RoombaStatus.Forward;

           //  control.wheel(150, 150);
        }



    }

    bool stopping=false;

    void stopRoomBa(){
       
        turnning=false;
            sendDatat(0, 0);
            status=RoombaStatus.Stop;

        


        if(!stopping){

            stopping=true;
            StartCoroutine(ToStopping());

        }



    }





    IEnumerator ToStopping(){

        yield return new WaitForSeconds(3);

        stopping=false;



    }






    public void sendString(string message)
    {
        //i=i+1;
        byte[] stringList = Encoding.UTF8.GetBytes(message);
        client.Send(stringList, stringList.Length, remoteEndPoint);
      //  print("message " + message + " sent to " + remoteEndPoint);
    }

}
