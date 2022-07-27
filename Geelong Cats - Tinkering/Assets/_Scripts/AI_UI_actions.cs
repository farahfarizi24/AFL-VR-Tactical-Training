using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AI_UI_actions : MonoBehaviour
{
    public GameObject baseUI;
    public Button Kicking;
    public Button Handball;
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

        Button handballbtn = Handball.GetComponent<Button>();
        kickbtn.onClick.AddListener(HandballTaskOnClick);

        Button movebtn = Move.GetComponent<Button>();
        kickbtn.onClick.AddListener(MoveTaskOnclick);
    }

    void KickingTaskOnClick()
    {

        Debug.Log("--- Kick initiated");
        AI_ballkick.SetTarget();
       // PerformTask("kicking");
        //Kick action

    }
   
    void HandballTaskOnClick()
    {
        Debug.Log("--- Handball initiated");

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
