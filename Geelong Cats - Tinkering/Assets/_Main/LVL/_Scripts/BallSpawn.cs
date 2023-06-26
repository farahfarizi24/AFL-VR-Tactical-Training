using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace com.DU.CE.LVL
{
    public class BallSpawn : MonoBehaviour
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
            GameObject SpawnerObject = GameObject.FindGameObjectWithTag("BallSpawner");

            SpawnerObject.GetComponent<LVL_BallSpawner>().m_isSpawned=false;
            SpawnerObject.GetComponent<LVL_BallSpawner>().Initialize();
           
        }

    }

}

