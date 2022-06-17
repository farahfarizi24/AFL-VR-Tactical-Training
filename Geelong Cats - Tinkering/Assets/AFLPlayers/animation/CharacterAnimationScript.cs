using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationScript : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private com.DU.CE.AI.AI_PathManager AI_Manager;

    public int CurrentAction;
    // 1 = idle , 2 = running, 3 = kicking, 4 = throwing, 
    [SerializeField] private Animator animator;
    [SerializeField]string CheckAnim;
    void Start()
    {
        animator = GetComponent<Animator>();
        CheckAnim = "None";
        CurrentAction = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (AI_Manager.isRunning == true)
        {
            CurrentAction = 2;
        }
    
    }


    void PlayAction()
    {
        if (CheckAnim != "Run" && CurrentAction == 2 || CheckAnim != "Idle" && CurrentAction ==1)
        {
           /* switch (CurrentAction)
            {
                case 1:
                    
                    animator.SetBool("IsIdle", true);
                    animator.SetBool("IsRunning", false);
                    CheckAnim = "Idle";

                    break;
                case 2:
                    animator.SetBool("IsRunning", true);
                    animator.SetBool("IsIdle", false);
                    CheckAnim = "Run";
                    break;
            }*/

        }


    }
}
