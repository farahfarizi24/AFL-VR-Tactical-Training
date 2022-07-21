using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Autohand;

public class BallKickerManager : MonoBehaviour
{
    // Public Variables
    public InputActionReference kickRef = null;
    public GameObject cursorPrefab;
    public Transform shootPoint;
    public LayerMask layer;
    public LineRenderer lineVisual;
    public float flightTime = 1f;
    public AudioSource audioSource;
    public Transform endPoint;
    public Color lineCanShootMaterial, lineCantShootMaterial, lineInvisible;
    // SphereCollider ColliderCursor = null;
    // Private Variables

    private GameObject EndPointCursor = null;
    private GameObject ball;
    private GameObject cursorInstance = null;
    private int lineSegment = 30;
    private bool buttonHeld = false;
    private bool buttonReleased = false;

    private EUSERHAND firstHeldHand = EUSERHAND.NONE;

    private void Awake()
    {
        // Registering callbacks for the input actions
        kickRef.action.started += ToggleInput;
        kickRef.action.canceled += ToggleInput;
    }

    private void OnDestroy()
    {
        kickRef.action.started -= ToggleInput;
        kickRef.action.canceled -= ToggleInput;
    }

    private void ToggleInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            buttonHeld = true;
            Debug.Log("context started");
        }
        else if (context.canceled)
        {
            Debug.Log("context Ended");
            buttonReleased = true;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        lineVisual.positionCount = lineSegment + 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        LaunchProjectile();


        if (!buttonHeld)
        {
            lineVisual.endColor = lineInvisible;
            lineVisual.startColor = lineInvisible;
        }
    }

    public void PopulateBallInfo(GameObject ballToAssign, EUSERHAND hand)
    {

        // TODO make kicking work from either hand
        firstHeldHand = hand;
        ball = ballToAssign;
        Debug.Log("Player: " + this.gameObject.transform.root.name + " has the ball");
    }

    public void DroppedBall()
    {
        //ball = null;
    }



    void LaunchProjectile()
    {
        // Check if holding the kick button and the ball isnt null
        if (buttonHeld && ball != null)
        {
            // Bit shift the index of the layer (19) to get a bit mask
            int layerMask = 1 << 19;

            // This would cast rays only against colliders in layer 19.
            // But instead we want to collide against everything except layer 19.
            layerMask = ~layerMask;

            RaycastHit hit;


            // Instantiate the cursor
            if (cursorInstance == null)
            {
                cursorInstance = Instantiate(cursorPrefab, Vector3.zero, Quaternion.identity);
     
            }

            // Does the raycast for the projectile hit a collider within the specified range?
            if (Physics.Raycast(shootPoint.position, shootPoint.TransformDirection(Vector3.forward), out hit, 75f, layerMask))
            {
                Debug.DrawRay(shootPoint.position, shootPoint.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

                endPoint.position = hit.point;
               
                Vector3 vo = CalculateVelocty(endPoint.position, shootPoint.position, flightTime);

                ChangeLineColor(lineCanShootMaterial);
                Visualize(vo, cursorInstance.transform.position); // Include the cursor position as the final nodes for the line visual position
              //  ColliderCursor.isTrigger = true;
             //   ColliderCursor.enabled = true;

                transform.rotation = Quaternion.LookRotation(vo);

                if (buttonReleased)
                {


                    // Kick the ball
                    EndPointCursor = Instantiate(cursorPrefab, endPoint.position, Quaternion.identity);
                    EndPointCursor.GetComponent<Collider>().enabled = true;
                    StartCoroutine(Kick(vo));
                   // EndPointCursor.GetComponent<Collider>().enabled = false;
                    buttonReleased = false;
                    buttonHeld = false;
                   
              //      ColliderCursor.enabled = false;
                }

            }
            // Not in range or suitable position so dont allow a kick to happen
            else
            {
                Debug.DrawRay(shootPoint.position, shootPoint.TransformDirection(Vector3.forward) * 1000, Color.white);
                Debug.Log("Did not Hit");

                ChangeLineColor(lineCantShootMaterial);

                if (buttonReleased)
                {
                    buttonHeld = false;
                    buttonReleased = false;
                }
            }

            cursorInstance.SetActive(true);
            cursorInstance.transform.position = endPoint.position + Vector3.up * 0.1f;
        }
    }

    private void ChangeLineColor(Color col)
    {
        lineVisual.endColor = col;
        lineVisual.startColor = col;
    }

    private IEnumerator Kick(Vector3 vo)
    {
        Debug.Log("Ball released");
        ball.GetComponent<Grabbable>().ForceHandsRelease();

        // Wait so ball has time to be released from hands
        yield return new WaitForSeconds(0.2f);
        Debug.Log("Ball kicked");

        ball.transform.position = shootPoint.position;
        Rigidbody obj = ball.GetComponent<Rigidbody>();

        obj.velocity = vo;
        buttonHeld = false;
        audioSource.Play();
        ball = null;

        cursorInstance.SetActive(false);
        Destroy(EndPointCursor);
        yield return null;
    }

    //added final position argument to draw the last line node to the actual target
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