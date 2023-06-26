using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace com.DU.CE.LVL
{
    public class BallSpawn : LVL_ASpawner
    {
        public GameObject Ball_prefab;
        public Transform spawnPoint;
        private Transform m_rugbyBall = null;

        public InputActionReference RightActionButtons;
        public InputActionReference LeftActionButtons;
      



        // Start is called before the first frame update
        private void Awake()
        {   RightActionButtons.action.started += SpawnBall;
            LeftActionButtons.action.started += SpawnBall;
          
        }

        private void OnDestroy()
        {
            LeftActionButtons.action.started -= SpawnBall;

            RightActionButtons.action.started -= SpawnBall;


        }

        public void SpawnBall(InputAction.CallbackContext context)
        {

          //  Ball_prefab = p_NetSpawnerSock.InstantiateNetObject(Ball_prefab.name, true, false);
            m_rugbyBall = p_NetSpawnerSock.InstantiateNetObject(Ball_prefab.name, true, false).transform;
            m_rugbyBall.transform.position = spawnPoint.position;
        }

        protected override IEnumerator OnCoachSetup()
        {
         
            yield return null;
        }

        protected override IEnumerator OnPlayerSetup()
        {
            yield return null;
        }
    }

}

