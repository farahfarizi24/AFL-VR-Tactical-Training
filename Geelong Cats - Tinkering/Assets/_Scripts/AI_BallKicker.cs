using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using System;
public class AI_BallKicker : MonoBehaviour
{

    //public GameObject cursorPrefab;
    public Transform shootPoint;
    private  Transform endPoint;
    public AI_UI_actions KickingAnimScript;
    public LineRenderer lineVisual;
    public bool HaveBall;
    public float flightTime = 1f;
  //  public CharacterAnimationScript CharaAnim;
    private int lineSegment = 30;
   [SerializeField] private GameObject cursorInstance=null; 
   private GameObject EndPointCursor = null;
    public InputActionReference RightTrigger;
    public InputActionReference LeftTrigger ;
    

    public BallCatch ThisAIBallOwnership;
    public Color lineCanShootMaterial, lineCantShootMaterial, lineInvisible;
    public GameObject ballObject;
    public GameObject BallAttach;
    public bool TargetSet = false;
    //For kinematic equations
    private Rigidbody ball;
    public float h = 3;
    public float gravity = -1;
    private Transform ShootTarget;
    private GameObject cursorCollider;
    // Start is called before the first frame update
    void Start()
    {

        cursorInstance = GameObject.FindGameObjectWithTag("Pointer");
        CheckBall();
    }
    
    private void Awake()
    {
        

            RightTrigger.action.started += RightTriggerPress;
            LeftTrigger.action.started += LeftTriggerPress;
        

    }
    private void OnDestroy()
    {

    
        RightTrigger.action.started -= RightTriggerPress;
        LeftTrigger.action.started -= LeftTriggerPress;
    }

    private void RightTriggerPress(InputAction.CallbackContext context)
    {
       if(cursorInstance!= null)
        {
            Debug.Log("LAUNCHBALL");
            LaunchBall();
        }
            
      
    }
    private void LeftTriggerPress(InputAction.CallbackContext context)
    {
       
          //  cursorInstance.transform.GetChild(0).gameObject.SetActive(false);


    }
    public void CheckBall()
    {
        if (ThisAIBallOwnership.BallHolder == true)
        {
            HaveBall = true;
        }
        else
        {
            HaveBall = false;
        }
    }

    
    public void SetTarget()
    {
        CheckBall();
        if (HaveBall)
        {


            ///Get cursor,
            cursorInstance = GameObject.FindGameObjectWithTag("Pointer");
            cursorInstance.transform.GetChild(0).gameObject.SetActive(true);
            cursorCollider = cursorInstance.transform.GetChild(0).gameObject;




        }
    }
  
    void LaunchBall()
    {
        cursorCollider.GetComponent<Collider>().enabled = true;
 
        ballObject = BallAttach.transform.GetChild(0).gameObject;

        //Unparent ball from the current object
        ballObject.transform.parent=null;
        //get rigidbody of ballobject
        // tag AI for not ballownership
        ThisAIBallOwnership.BallHolder = false ;
        //Move ball towards the kick point(shoot point) and play the animation//

         
        ///////////////////////////////////////////
        ball = ballObject.GetComponent<Rigidbody>();
        ball.isKinematic = false;
        //   GameObject ballObject = transform.parent.FindChild.tag(tag)

        //Get target for shooting
        if (TargetSet == false)
        {

            ShootTarget = cursorInstance.transform;
            TargetSet = true;
        }

        Vector3 vo = CalculateVelocty(ShootTarget.position, shootPoint.position, flightTime);
        StartCoroutine(Kick(vo));


    }

    private IEnumerator Kick(Vector3 vo)
    {
        Debug.Log("Ball released");

        // Wait so ball has time to be released from hands

        ///ADD CODE FOR THE KICKING AND BALL MOVEMENT HERE
        KickingAnimScript.PerformTask("kicking");
        //wait until perfect time to move ball to position
        yield return new WaitForSeconds(1.5f);
        Debug.Log("Ball kicked");
     
      
        ball.transform.position = shootPoint.position;
      
        Rigidbody obj = ball.GetComponent<Rigidbody>();
        
        obj.velocity = vo;

        //  audioSource.Play();
        ball = null;

        
        
        cursorCollider.GetComponent<Collider>().enabled = false;
        cursorCollider.SetActive(false);
        cursorInstance = null;
        //cursorInstance.SetActive(false);
        yield return null;
    }

   

    
    Vector3 CalculateVelocty(Vector3 target, Vector3 origin, float time)
    {
        Vector3 distance = target - origin;
        Vector3 distanceXz = distance;
        distanceXz.y = 0f;

        float sY = distance.y;
        float sXz = distanceXz.magnitude;

        float Vxz = sXz / time;
        float Vy = (sY / time) + (0.5f * Mathf.Abs(Physics.gravity.y) * time);

        Vector3 result = distanceXz.normalized;
        result *= Vxz;
        result.y = Vy;

        return result;
    }

}
