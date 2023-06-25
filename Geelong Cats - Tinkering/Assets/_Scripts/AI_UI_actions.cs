using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class AI_UI_actions : MonoBehaviour
{
    public GameObject PossessionPrompt;
    public GameObject ThisUserObject=null;
    public GameObject ThisObjParent = null;
    public GameObject ThisAIObject;
    public bool CurrentlyPossessing;
    public GameObject baseUI;
    public Button Kicking;
    public Button Possess;
    public Button Move;
    public Animator anim;
    private IEnumerator TaskCourutine;
    public AI_BallKicker AI_ballkick;
    public GameObject child = null;
    public InputActionReference RightTrigger;
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

    private void Awake()
    {
        RightTrigger.action.started += PossessionExit;
    }
    private void PossessionExit(InputAction.CallbackContext context)
    {
        if(context.started && CurrentlyPossessing)
        {
          
           // ThisAIObject.transform.SetParent(ThisObjParent.transform);
            PossessionPrompt.SetActive(false);
           // ThisAIObject = null;
           // ThisObjParent = null;
            ThisAIObject.SetActive(true);
            CurrentlyPossessing = false;
           // child.SetActive(true);
          //  child = null;
        }
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
       
       
        ThisUserObject = GameObject.FindGameObjectWithTag("Player");
        ThisUserObject.transform.position = new Vector3
            (ThisAIObject.transform.position.x,
             ThisUserObject.transform.position.y, ThisAIObject.transform.position.z+0.5f);

        ThisAIObject.SetActive(false);
        CurrentlyPossessing = true;
        PossessionPrompt.SetActive(true);
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
