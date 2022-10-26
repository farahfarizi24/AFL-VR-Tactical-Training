using UnityEngine;
using UnityEngine.AI;
public class CharacterAnimationScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private BallCatch BallOwnershipManager;
    [SerializeField] private com.DU.CE.AI.AI_PathManager AI_Manager;
    [SerializeField] public NavMeshAgent character;
    public int CurrentAction;
    // 1 = idle , 2 = running, 3 = kicking, 4 = throwing, 
    [SerializeField] private Animator animator;
    [SerializeField] string CheckAnim;

    private const string IsRunning = "IsRunning";
    private const string IsIdle = "IsIdle";
    private const string IsHoldingBall = "HoldBall";
    public bool isBallHolder = false;

    private Vector3 previous;
    private float speed;

    void Start()
    {

        animator = GetComponent<Animator>();
        character = GetComponent<NavMeshAgent>();
        ToggleIdle();
        animator.SetLayerWeight(animator.GetLayerIndex("ArmLayer"), 1f);
        animator.SetLayerWeight(animator.GetLayerIndex("BodyLayer"), 1f);

    }

    // Update is called once per frame
    void Update()
    {
        updateVelocity();

        if (speed > 0.01f)
        {
            ToggleRun();
        }
        else
        {
            ToggleIdle();
        }


    }

    private void updateVelocity()
    {
        // calculate velocity by position
        speed = ((transform.position - previous).magnitude) / Time.deltaTime;
        previous = transform.position;
       // Debug.Log("speed" + speed.ToString());

    }

    public void ToggleRun()
    {
        // if (character.remainingDistance > 0.1f)
        animator.SetBool(IsRunning, true);
        animator.SetBool(IsIdle, false);
        CheckBallOwnership();

        Debug.Log("Run Toggled");
    }

    public void ToggleIdle()
    {
        Debug.Log("Idle Toggled");
        animator.SetBool(IsRunning, false);
        animator.SetBool(IsIdle, true);

        CheckBallOwnership();

    }

    public void CheckBallOwnership()
    {

        if (isBallHolder == true && BallOwnershipManager.BallHolder == false)
        {
            NotHoldingBall();
        }
    }

    public void NotHoldingBall()
    {
        Debug.Log("HOLDBALL FALSE");
        animator.SetBool(IsHoldingBall, false);
        isBallHolder = false;
    }


}
