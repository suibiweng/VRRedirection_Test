using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Redirection;

public class DogBehaviourRoomBa : MonoBehaviour
{
    public ZigZagRedirector zrdirect;
   public  SimulationManager simulationManager;
    public RoombaManager manager;
    public MoveRoomba moveRoomba;
    public bool pat=false;
    public bool isOnTracker;

    public Transform RDWT;
    public float speed;


    public Transform Head;



    public GameObject Rhand,Lhand;


    public Transform model;
    public float direction=1;

    Vector3 Targetposition;
    Vector3 OldTarget;

    public string wayPointName;

    public GameObject waypoint;

    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
                 Rhand=GameObject.FindWithTag("rightHand");
            Lhand=GameObject.FindWithTag("leftHand");
   
        animator=model.GetComponent<Animator>();

        simulationManager=FindObjectOfType<SimulationManager>().GetComponent<SimulationManager>();
        
    }

    // Update is called once per frame

    bool sendCmd;
    void Update()
    {

        waypoint=GameObject.Find("TrackerTarget");
        zrdirect=GameObject.FindObjectOfType<ZigZagRedirector>().GetComponent<ZigZagRedirector>();



        if (waypoint==null)return;




        Targetposition=new Vector3(waypoint.transform.position.x,transform.position.y,waypoint.transform.position.z);


        if(Input.GetKeyDown(KeyCode.Space)){


         
                simulationManager.updateWaypoint();

               StartCoroutine(delayCallRoomba());

        }




           // print("right hand"+ Vector3.Distance(transform.position,Rhand.transform.position));
           //  print("left hand"+ Vector3.Distance(transform.position,Lhand.transform.position));
        
    




        if(Vector3.Distance(transform.position,Targetposition)<1f){

            direction=0;


      pat = Vector3.Distance(transform.position,Rhand.transform.position)<=1.2f ||  Vector3.Distance(transform.position, Lhand.transform.position)<=1.2f; 
   



        }else{
            direction=1;

pat=false;   
        }




         if(direction>0){

         
            transform.LookAt(new Vector3(Targetposition.x,transform.position.y,Targetposition.z));    
            

         }
            
        
     
        transform.Translate(Vector3.forward*speed*direction);



        AnimationControl((int)direction);

        /*

        if(Vector3.Distance(Head.position,model.position)<0.5f){

            if(direction<1){
                zrdirect.ZRedirection();
                StartCoroutine(delayCallRoomba());
            }

        }*/

        
    }







    void AnimationControl(int dir){
        switch (dir){
            case 0:
            animator.SetBool("Run",false);
            break;

            case 1:
            animator.SetBool("Run",true);
            break;

        }





    }


    public void TargetChange(){

        StartCoroutine(delayCallRoomba());


    }







    IEnumerator delayCallRoomba(){

        yield return new WaitForSeconds(Random.Range(5,10));

        //manager.RoombaMove();
       moveRoomba.MoveBackandForward();

    }
}
