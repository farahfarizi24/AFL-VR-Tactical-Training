using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCatch : MonoBehaviour
{
    public Rigidbody rb;
    public Rigidbody mainBodyrb;
    public Animator anim;
    [SerializeField] private bool BallCatcher;
    public GameObject BallHoldPoint;
    public GameObject BallOwnership;
    public GameObject MainBody;
    public bool BallHolder; //to activate character animation script
    private IEnumerator Courutine;
    public float speed = 1.0f;
    // Start is called before the first frame update
    CharacterAnimationScript animationController;
    private const string HoldBall = "HoldBall";
    void Start()
    {
        BallCatcher = true;    }

    // Update is called once per frame
    void Update()
    {
        /*if (BallCatcher== true)
        {
            var move = speed * Time.deltaTime; //calculate the distance to move
            BallOwnership.transform.position = Vector3.MoveTowards
                (BallOwnership.transform.position, transform.position, move);

           // if(Vector3.Distance(BallOwnership.transform.position, transform.position) < 0.001f)
          //  {
             //   BallOwnership.transform.position =transform.position;
          //  }
        }*/

    }

    private void OnTriggerEnter(Collider collider)
    {
       
            if (collider.gameObject.CompareTag("Ball") && BallCatcher == true)
            {
            // turn on kinematics so player is not influenced by the ball
            rb = collider.transform.parent.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            mainBodyrb = MainBody.GetComponent<Rigidbody>();
            mainBodyrb.isKinematic = true;

            //change ball ownership
            BallOwnership = collider.transform.parent.gameObject;
            Debug.Log(" This person is holding Ball");

            BallHolder = true;

            anim.SetLayerWeight(anim.GetLayerIndex("ActionLayer"), 1f);
            anim.SetLayerWeight(anim.GetLayerIndex("ArmLayer"), 0f);
            anim.SetLayerWeight(anim.GetLayerIndex("BodyLayer"), 0f);
            Debug.Log("Layerweight complete");

            if(BallOwnership.transform.position.y> 1.5)
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
    
            //BallOwnership.transform.localPosition = BallHoldPoint.transform.localPosition;


            // Set Ball as a child of a hand object
           
            } 
 
    }

    IEnumerator WaitForActionAnimToFinish(string TriggerName)
    {
        float animationLength = anim.GetCurrentAnimatorStateInfo(1).length + anim.GetCurrentAnimatorStateInfo(1).normalizedTime;
        //TODO on here, make ball move towards the player
        yield return new WaitForSeconds(2.5f);
        SetBallOwnership();
        anim.ResetTrigger(TriggerName);
       
        
    }
    public void SetBallOwnership()
    {

       // BallOwnership = collider.transform.parent.gameObject;
        BallOwnership.transform.position = BallHoldPoint.transform.position;
        BallOwnership.transform.parent = BallHoldPoint.transform;
        anim.SetBool(HoldBall, true);
        Debug.Log("Ball ownership completed");
        anim.SetLayerWeight(anim.GetLayerIndex("BodyLayer"), 1f);
        anim.SetLayerWeight(anim.GetLayerIndex("ArmLayer"), 1f);

        BallCatcher = false; //stop from repeating

        anim.SetLayerWeight(anim.GetLayerIndex("ActionLayer"), 0f);
    }
        

}
