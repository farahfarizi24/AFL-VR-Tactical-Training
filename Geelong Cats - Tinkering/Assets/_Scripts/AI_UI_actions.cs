using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AI_UI_actions : MonoBehaviour
{
    public GameObject ThisUserObject=null;
    public GameObject baseUI;
    public Button Kicking;
    public Button Possess;
    public Button Move;
    public Animator anim;
    private IEnumerator TaskCourutine;
    public AI_BallKicker AI_ballkick;
    // Start is called before the first frame update
    void Start()
    {
        baseUI.SetActive(false);
        Button kickbtn = Kicking.GetComponent<Button>();
        kickbtn.onClick.AddListener(KickingTaskOnClick);

        Button possessbtn = Possess.GetComponent<Button>();
       possessbtn.onClick.AddListener(PossessTaskOnClick);

        Button movebtn = Move.GetComponent<Button>();
        movebtn.onClick.AddListener(MoveTaskOnclick);
    }

    void KickingTaskOnClick()
    {

        Debug.Log("--- Kick initiated");
        AI_ballkick.SetTarget();
       // PerformTask("kicking");
        //Kick action

    }
   
    void PossessTaskOnClick()
    {
        Debug.Log("--- Possess initiated");
        GameObject ThisAIObject = transform.parent.parent.gameObject;
        ThisUserObject = GameObject.FindGameObjectWithTag("Player");
        ThisUserObject.transform.position = new Vector3
            (ThisAIObject.transform.position.x,
             ThisUserObject.transform.position.y, ThisAIObject.transform.position.z);

        //transform rotation
        ThisAIObject.transform.SetParent(ThisUserObject.transform);

       //CHECK HOW DO THE AI GET DESTROYED BY Mike probably it was getting deactivated not destroyed
       //Instead of destroy, disable player object. Put notification that atm it's currently possessing player
       // Give them ability to stop possessing player perhaps press "A" again to stop 
        // Destroy(ThisAIObject);
        // Destroy(transform.parent.parent.gameObject);
        // Move playable player here
        // disable this AI
    }

    void MoveTaskOnclick()
    {
        Debug.Log("--- Move initiated");
    }


    public void PerformTask(string task)
    {
        anim.SetLayerWeight(anim.GetLayerIndex("ActionLayer"), 1f);
        anim.SetLayerWeight(anim.GetLayerIndex("ArmLayer"), 0f);
        anim.SetLayerWeight(anim.GetLayerIndex("BodyLayer"), 0f);

        if(task == "kicking")
        {
            anim.SetTrigger("Kick");
            TaskCourutine = WaitForActionToFinish("Kick");
            StartCoroutine(TaskCourutine);

        }else if(task == "handball")
        {
            anim.SetTrigger("Handball");
            TaskCourutine = WaitForActionToFinish("Handball");
            StartCoroutine(TaskCourutine);
        }
    }


    IEnumerator WaitForActionToFinish(string triggername)
    {
        if(triggername == "Kick")
        {
            yield return new WaitForSeconds(2.0f);
            const string IsHoldingBall = "HoldBall";
            anim.SetBool(IsHoldingBall, false);
        }
        else if(triggername == "Handball"){
            yield return new WaitForSeconds(1.0f);
        }
     


        anim.SetLayerWeight(anim.GetLayerIndex("BodyLayer"), 1f);
        anim.SetLayerWeight(anim.GetLayerIndex("ArmLayer"), 1f);
        anim.SetLayerWeight(anim.GetLayerIndex("ActionLayer"), 0f);
       
        anim.ResetTrigger(triggername);

        //OVER HERE PERFORM THE kicking action and unparent the ball
    }
}
