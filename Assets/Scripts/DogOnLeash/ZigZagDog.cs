using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum DogMissionStatus{
    Sit,AtoB,BtoA

}

public enum Dogstatus{
    Sit,Moving


}


public class ZigZagDog : MonoBehaviour
{
    Animator animator;


    public DogMissionStatus mission;

    public Dogstatus status;

    public float speed=0.01f;
    public GameObject [] RealTargets;
    public bool spotTracking;
    public Transform Target;

    public SendOsctoSpot spotOsc;

    // Start is called before the first frame update
    void Start()
    {
        animator=GetComponent<Animator>();
        StartCoroutine(getRealTargets());



        spotOsc =FindObjectOfType<SendOsctoSpot>();

    }

    IEnumerator getRealTargets(){
        yield return new WaitForSeconds(1f);

        RealTargets=GameObject.FindGameObjectsWithTag("RealTarget");
        switchTarget();



     


    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){

            status=Dogstatus.Moving;

        }




        
        switch (status){
            case Dogstatus.Sit:
                animator.SetBool("isWalking",false);
                animator.SetBool("isSitting",true);
            
            break;

            case Dogstatus.Moving:

                animator.SetBool("isWalking",true);
                animator.SetBool("isSitting",false);
                faceToTarget();
                MoveToTarget();
            
            break;


        }

      








/*

        switch (mission){

            case DogMissionStatus.Sit:
            animator.SetBool("isWalking",false);
            animator.SetBool("isSitting",true);

            break;

            case DogMissionStatus.AtoB:
                animator.SetBool("isWalking",true);
                  animator.SetBool("isSitting",false);
                faceToTarget();
                MoveToTarget();
              //  NeartoWhichTargets();



            break;



            case DogMissionStatus.BtoA:
            animator.SetBool("isWalking",true);
             animator.SetBool("isSitting",false);
                faceToTarget();
                MoveToTarget();
               // NeartoWhichTargets();

            
            
            break;


        }
        
*/




    
    
    }

    void faceToTarget(){
          transform.LookAt( new Vector3(Target.position.x,transform.position.y,Target.position.z) );




    }

    int OscIndex;

    void switchTarget(){

       if(NeartoWhichTargets()==1){setMoveTarget(0);
       OscIndex=2;
       }


       if(NeartoWhichTargets()==0){setMoveTarget(1);
       OscIndex=1;
       }




    }


    public void MoveToTarget(){

        faceToTarget();

        if((Vector3.Distance(transform.position,
        Target.position)<nearRange)){

                  status=Dogstatus.Sit;

                  spotOsc.oscSend(0);
                  
                  
                   NeartoWhichTargets();
                   switchTarget();
        }
      if(!spotTracking)  transform.Translate(new Vector3(0,0,speed));

        



    }


    void setMoveTarget(int targetIndex){

        Target=RealTargets[targetIndex].transform;

    }

    public float nearRange=1f;

    int NeartoWhichTargets(){

        if(Vector3.Distance(transform.position,
            RealTargets[0].transform.position)<nearRange)
        {

     
            return 0;

        }


        if(Vector3.Distance(transform.position,
        RealTargets[1].transform.position)<nearRange)
        {
          
            return 1;

        }



     
            return -1;

        



     }


    bool Touchonce;

    void OnCollisionEnter(Collision other) {
    if(other.gameObject.tag == "RightHand" ||other.gameObject.tag == "LeftHand"){
        if(!Touchonce && status==Dogstatus.Sit){
            Touchonce=true;
            
             
            StartCoroutine( ReciveTouch());

            


        }
     }

    }


    IEnumerator ReciveTouch(){
        yield return new WaitForSeconds(1.5f);
        status=Dogstatus.Moving;

        spotOsc.oscSend(OscIndex);


    }    



    void OnCollisionExit(Collision other) {
    if(other.gameObject.tag == "RightHand" ||other.gameObject.tag == "LeftHand"){

        Touchonce=false;
     }

    }

            


    
}
