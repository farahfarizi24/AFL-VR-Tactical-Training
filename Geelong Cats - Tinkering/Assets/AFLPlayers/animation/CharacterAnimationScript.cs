using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class CharacterAnimationScript : MonoBehaviour
{
    // Start is called before the first frame update
    BallCatch BallOwnershipManager;
    [SerializeField] private com.DU.CE.AI.AI_PathManager AI_Manager;
    [SerializeField]    private NavMeshAgent character;
    public int CurrentAction;
    // 1 = idle , 2 = running, 3 = kicking, 4 = throwing, 
    [SerializeField] private Animator animator;
    [SerializeField]string CheckAnim;

    private const string IsRunning = "IsRunning";
    private const string IsIdle = "IsIdle";
    private const string IsHoldingBall = "HoldBall";
    void Start()
    {
        animator = GetComponent<Animator>();
        CheckAnim = "None";
        animator.SetBool(IsIdle, true);
        animator.SetBool(IsRunning, false);
        animator.SetLayerWeight(animator.GetLayerIndex("ArmLayer"), 1f);
        animator.SetLayerWeight(animator.GetLayerIndex("BodyLayer"), 1f);

    }

    // Update is called once per frame
    void Update()
    {

        ///Check if character is currently moving
      
        
        if (character.remainingDistance > 0.1f)
        {
         
            animator.SetBool(IsRunning, true);
            animator.SetBool(IsIdle, false);
            CheckBallOwnership();

        }
        else
        {

            animator.SetBool(IsRunning, false);
            animator.SetBool(IsIdle, true);


            CheckBallOwnership();
        }

   
    }

    private void CheckBallOwnership()
    {
        if (BallOwnershipManager.BallHolder == false)
        {
            Debug.Log("HOLDBALL FALSE");
            animator.SetBool(IsHoldingBall, false);
        }
    }

  
}
