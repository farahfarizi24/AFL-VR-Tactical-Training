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
   
    public LineRenderer lineVisual;
    public bool HaveBall;
    public float flightTime = 1f;

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

    // Start is called before the first frame update
    void Start()
    {

   
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
       
            cursorInstance.transform.GetChild(0).gameObject.SetActive(false);


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

         


        }
    }
  
    void LaunchBall()
    {
        ballObject = BallAttach.transform.GetChild(0).gameObject;

        //Unparent ball from the current object
        ballObject.transform.parent=null;
        //get rigidbody of ballobject
        // tag AI for not ballownership
        ThisAIBallOwnership.BallHolder = false ;
        ball = ballObject.GetComponent<Rigidbody>();
        ball.isKinematic = false;
        //   GameObject ballObject = transform.parent.FindChild.tag(tag)

        //Get target for shooting
        if (TargetSet == false)
        {

            ShootTarget = cursorInstance.transform;
            TargetSet = true;
        }
        Physics.gravity = Vector3.up * gravity;
        ball.useGravity = true;
        
        ball.velocity = CalculateLaunchVelocity();
        Debug.Log("Launch Velocity =" + CalculateLaunchVelocity());
    }

    Vector3 CalculateLaunchVelocity()
    {
        float displacementY = ShootTarget.position.y - ball.position.y;
        Vector3 displacementXZ = new Vector3(ShootTarget.position.x - ball.position.x, 0, ShootTarget.position.z - ball.position.z);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * h);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2 * (displacementY - h) / gravity));
        return velocityXZ + velocityY;
    }

    /*

    void Visualize(Vector3 vo, Vector3 finalPos)
    {
        for (int i = 0; i < lineSegment; i++)
        {
            Vector3 pos = CalculatePosInTime(vo, (i / (float)lineSegment) * flightTime);
            lineVisual.SetPosition(i, pos);
        }

        lineVisual.SetPosition(lineSegment, finalPos);
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
    private void ChangeLineColor(Color col)
    {
        lineVisual.endColor = col;
        lineVisual.startColor = col;
    }
    // Update is called once per frame
 
    Vector3 CalculatePosInTime(Vector3 vo, float time)
    {
        Vector3 Vxz = vo;
        Vxz.y = 0f;

        Vector3 result = shootPoint.position + vo * time;
        float sY = (-0.5f * Mathf.Abs(Physics.gravity.y) * (time * time)) + (vo.y * time) + shootPoint.position.y;

        result.y = sY;

        return result;
    }*/
}
