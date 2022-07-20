using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using System;
public class AI_BallKicker : MonoBehaviour
{

    public GameObject cursorPrefab;
    public Transform shootPoint;
    public Transform endPoint;
    public LineRenderer lineVisual;
    public bool HaveBall;
    public float flightTime = 1f;

    private int lineSegment = 30;
   [SerializeField] private GameObject cursorInstance=null; 
    private GameObject EndPointCursor = null;
    public InputActionReference RightTrigger=null;
    public InputActionReference LeftTrigger = null;
    public BallCatch ThisAIBallOwnership;
    public Color lineCanShootMaterial, lineCantShootMaterial, lineInvisible;
    // Start is called before the first frame update
    void Start()
    {
        //get haveball
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
       
            Debug.Log("LAUNCHBALL");
        
      
    }
    private void LeftTriggerPress(InputAction.CallbackContext context)
    {
       
            cursorInstance.transform.GetChild(0).gameObject.SetActive(false);
        
        
    }
    public void CheckBall()
    {
        if(ThisAIBallOwnership.BallHolder == true)
        {
            HaveBall = true;
        }
        else
        {
            HaveBall=false;
        }
    }
    public void SetTarget()
    {
        CheckBall();
        if (HaveBall)
        {
            // Bit shift the index of the layer (19) to get a bit mask
            int layerMask = 1 << 19;

            // This would cast rays only against colliders in layer 19.
            // But instead we want to collide against everything except layer 19.
            layerMask = ~layerMask;

            RaycastHit hit;

            ///Get cursor,
            cursorInstance = GameObject.FindGameObjectWithTag("Pointer");
            cursorInstance.transform.GetChild(0).gameObject.SetActive(true);


            ///IF button trigger is pressed then launch ball

            ////BEN CODE

      /*
            if (Physics.Raycast(shootPoint.position, shootPoint.TransformDirection(Vector3.forward), out hit, 75f, layerMask))
            {
                Debug.DrawRay(shootPoint.position, shootPoint.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

                endPoint.position = hit.point;

                Vector3 vo = CalculateVelocty(endPoint.position, shootPoint.position, flightTime);

                ChangeLineColor(lineCanShootMaterial);
              //  Visualize(vo, cursorInstance.transform.position); // Include the cursor position as the final nodes for the line visual position
                                                                  //  ColliderCursor.isTrigger = true;
                                                                  //   ColliderCursor.enabled = true;

             //   transform.rotation = Quaternion.LookRotation(vo);
                /////// THIS WILL NEED TO BE CHANGED AS WE NEED THE END POINT CURSOR TO ACTUALLY MATCHES THE point
              /*  if (buttonReleased)
                {


                    // Kick the ball
                    EndPointCursor = Instantiate(cursorPrefab, endPoint.position, Quaternion.identity);
                    EndPointCursor.GetComponent<Collider>().enabled = true;
                    StartCoroutine(Kick(vo));
                  
                    buttonReleased = false;
                    buttonHeld = false;

                 
                }*/
              /////////////////////////////
          //  }
        }
    }
  

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
    void Update()
    {
        
    }
    Vector3 CalculatePosInTime(Vector3 vo, float time)
    {
        Vector3 Vxz = vo;
        Vxz.y = 0f;

        Vector3 result = shootPoint.position + vo * time;
        float sY = (-0.5f * Mathf.Abs(Physics.gravity.y) * (time * time)) + (vo.y * time) + shootPoint.position.y;

        result.y = sY;

        return result;
    }
}
