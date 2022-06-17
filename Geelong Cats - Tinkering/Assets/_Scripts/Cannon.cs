using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Autohand;

namespace com.DU.CE.LVL
{
    public class Cannon : LVL_ASpawner
    {

        public Transform shootPoint;

        public float flightTime = 3f;
        public AudioSource audioSource;
        public Transform endPoint;
        public GameObject ball;


        private float currentTime = 0;
        public float howOftenToShoot = 0;
        public float firstTimeDelay = 5;

        private bool canShoot = false;

        private void Awake()
        {
            // Registering callbacks for the input actions

        }

        private void OnDestroy()
        {

        }



        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (canShoot)
            {


                currentTime += Time.deltaTime;

                if (currentTime >= howOftenToShoot + firstTimeDelay)
                {

                    LaunchProjectile();
                    currentTime = 0;
                    firstTimeDelay = 0;
                }


            }
        }

        internal void InitializeBall()
        {


            ball = p_NetSpawnerSock.InstantiateNetObject("INT_RugbyBall 1", true, false);

        }


        public void DroppedBall()
        {
            //ball = null;
        }



        void LaunchProjectile()
        {
            Vector3 vo = CalculateVelocty(endPoint.position, shootPoint.position, flightTime);
            InitializeBall();
            //transform.rotation = Quaternion.LookRotation(vo);

            StartCoroutine(Shoot(vo));
        }



        private IEnumerator Shoot(Vector3 vo)
        {
            Debug.Log("Ball Shot");


            //ball.transform.position = spawnPoint.position;

            ball.transform.position = shootPoint.position;
            Rigidbody obj = ball.GetComponent<Rigidbody>();
            obj.velocity = vo;
            //audioSource.Play();


            yield return null;
        }

        Vector3 CalculateVelocty(Vector3 target, Vector3 origin, float time)
        {
            Vector3 distance = target - origin;
            Vector3 distanceXz = distance;
            distanceXz.y = 0f;

            float sY = distance.y;
            float sXz = distanceXz.magnitude;

            float Vxz = sXz / time;
            float Vy = (sY / time) + (0.5f * Mathf.Abs(Physics.gravity.y) * time);

            Vector3 result = distanceXz.normalized;
            result *= Vxz;
            result.y = Vy;

            return result;
        }

        protected override IEnumerator OnCoachSetup()
        {
            canShoot = true;
            yield return null;
        }

        protected override IEnumerator OnPlayerSetup()
        {
            yield return null;
        }
    }
}