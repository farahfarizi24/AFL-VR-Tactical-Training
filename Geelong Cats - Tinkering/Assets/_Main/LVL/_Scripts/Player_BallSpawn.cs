using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Autohand;



namespace com.DU.CE.LVL
{

    public class Player_BallSpawn : LVL_ASpawner
    {

        public Transform shootPoint;
        public GameObject ball;
        public InputActionReference RightButtons;
        public InputActionReference LeftButtons;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void Awake()
        {


            RightButtons.action.started += CreateNewBall;
            LeftButtons.action.started += CreateNewBall;


        }
        private void OnDestroy()
        {


            RightButtons.action.started -= CreateNewBall;
            LeftButtons.action.started -= CreateNewBall;
        }

        public void CreateNewBall(InputAction.CallbackContext context)
        {
            SpawnBall();
        }
        internal void SpawnBall()
        {

            ball = p_NetSpawnerSock.InstantiateNetObject("AFLBALL_Active", true, false);
            ball.transform.position = shootPoint.position;
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
