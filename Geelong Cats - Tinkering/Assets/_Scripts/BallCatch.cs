using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCatch : MonoBehaviour
{
    private Rigidbody rb = null;
    public Rigidbody mainBodyrb;
    public Animator anim;
    [SerializeField] public bool BallCatcher;
    public GameObject BallHoldPoint;
    public GameObject BallOwnership;
    public GameObject MainBody;
    public bool BallHolder; //to activate character animation script
    private IEnumerator Courutine;
    public float speed = 1.0f;
    // Start is called before the first frame update

    public GameObject BallDestination;
    public BallSensor ballSensor;
    private const string HoldBall = "HoldBall";
    void Start()
    {
        //BallCatcher = true;
        BallHolder = false; }

    // Update is called once per frame
    void Update()
    {


    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("BallDestination")) { BallCatcher = true; Debug.Log("Ball Destination entered, catcher true");
            BallDestination = collider.gameObject;
            SetCatchingAnim();
        }

        if (collider.gameObject.CompareTag("Ball") && BallCatcher == true)
        {
            mainBodyrb = MainBody.GetComponent<Rigidbody>();
            rb = collider.GetComponentInParent<Rigidbody>();
         
            //change ball ownership
            BallOwnership = collider.transform.parent.gameObject;
            //Debug.Log(" This person is holding Ball");

            BallHolder = true;

            //  Debug.Log("Layerweight complete");

            
      

            //BallOwnership.transform.localPosition = BallHoldPoint.transform.localPosition;
            //wait until Ball is triggering another sensor
            if (ballSensor.SensorTrigger == true)
            {
                Debug.Log("IE numerator check sensor trigger");
                SetBallOwnership();

            }

            // Set Ball as a child of a hand object

        }

    }

    private void OnTriggerExit(Collider other)
    {
     //   if (other.gameObject.CompareTag("BallDestination")) { BallCatcher = false; Debug.Log("Ball Destination exited, catcher false"); }
       
    }
    public void SetCatchingAnim(){

        anim.SetLayerWeight(anim.GetLayerIndex("ActionLayer"), 1f);
        anim.SetLayerWeight(anim.GetLayerIndex("ArmLayer"), 0f);
        anim.SetLayerWeight(anim.GetLayerIndex("BodyLayer"), 0f);

        if (BallDestination.transform.position.y > 1.5)
        {
            anim.SetTrigger("HighCatch");
            Courutine = WaitForActionAnimToFinish("HighCatch");
            StartCoroutine(Courutine);
        }
        else
        {

            anim.SetTrigger("LowCatch");
            Courutine = WaitForActionAnimToFinish("LowCatch");
            StartCoroutine(Courutine);
        }
    }
    IEnumerator WaitForActionAnimToFinish(string TriggerName)
    {
       // float animationLength = anim.GetCurrentAnimatorStateInfo(1).length + anim.GetCurrentAnimatorStateInfo(1).normalizedTime;
        //TODO on here, make ball move towards the player

      

        yield return new WaitForSeconds(2.5f);
       //Ball ownership is triggered from characteranimationscript.cs

      
       

        anim.SetLayerWeight(anim.GetLayerIndex("BodyLayer"), 1f);
        anim.SetLayerWeight(anim.GetLayerIndex("ArmLayer"), 1f);

        BallCatcher = false; //stop from repeating

        anim.SetLayerWeight(anim.GetLayerIndex("ActionLayer"), 0f);
        anim.ResetTrigger(TriggerName);
        


    }
    public void SetBallOwnership()
    {

        // turn on kinematics so player is not influenced by the ball
       
        rb.isKinematic = true;
     
        mainBodyrb.isKinematic = true;

        // BallOwnership = collider.transform.parent.gameObject;
        BallOwnership.transform.position = BallHoldPoint.transform.position;
        BallOwnership.transform.parent = BallHoldPoint.transform;
        anim.SetBool(HoldBall, true);
        Debug.Log("Ball ownership completed");
     
    }
        

}
