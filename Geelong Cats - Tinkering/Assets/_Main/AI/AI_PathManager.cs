using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.AI;

namespace com.DU.CE.AI
{
    public class AI_PathManager : MonoBehaviour
    {
        private NavMeshAgent navMeshAgent;
        [SerializeField] private Queue<Vector3> pathPoints = new Queue<Vector3>();

        public bool isRunning;

        [SerializeField] private CharacterAnimationScript animScript;
        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            //FindObjectOfType<LEVEL_Manager>().OnScenarioStart += StartMoving;
        }

        public void SetPoints(IEnumerable<Vector3> points)
        {
            pathPoints = new Queue<Vector3>(points);
        }

        void Update()
        {
            //UpdatePathing();
        }

        void StartMoving()
        {
            //needsToMove = true;
            Debug.Log("---------Starting Coroutine");
          
            StartCoroutine(UpdatePathing());
          
        }


        IEnumerator UpdatePathing()
        {
            Debug.Log("---------Coroutine Started");
            while (pathPoints.Count != 0)
            {
                
                if (!navMeshAgent.hasPath || navMeshAgent.remainingDistance < 0.5f)
                {
                   
                    Debug.Log("----------Setting Destination " + pathPoints.Peek());
                    isRunning = true;
                    animScript.ToggleRun();
                    navMeshAgent.SetDestination(pathPoints.Dequeue());
                   

                }
               // isRunning= false;
               // animScript.CurrentAction = 1;
                yield return null;
            }
            isRunning = false;
            //StopCoroutine(UpdatePathing());
        }

        private bool ShouldSetDestination()
        {
            if (pathPoints.Count == 0)
                return false;

            else if (navMeshAgent.hasPath == false || navMeshAgent.remainingDistance < 0.5f)
                return true;

            return false;
        }
    }
}
