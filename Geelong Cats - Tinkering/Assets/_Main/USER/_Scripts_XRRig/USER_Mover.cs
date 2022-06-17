using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace com.DU.CE.USER
{
    public class USER_Mover : MonoBehaviour
    {
        [Header("Controllers")]
        public InputActionProperty MoveAction;
        public InputActionProperty TurnAction;

        [Header("Body")]
        public GameObject head;
        private CharacterController controller;

        [Header("Settings")]
        public bool snapTurning;
        public float turnAngle;
        public float speed = 5;
        public float gravity = 1;

        private float currentGravity = 0;

        //private bool axisReset = true;

        Vector3 moveAxis;
        Vector2 turningAxis;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
        }

        private void OnEnable()
        {
            MoveAction.action.performed += OnMove;
            TurnAction.action.performed += OnTurn;
        }

        private void OnDisable()
        {
            MoveAction.action.performed -= OnMove;
            TurnAction.action.performed -= OnTurn;
        }



        private void OnMove(InputAction.CallbackContext obj)
        {
            moveAxis = obj.action.ReadValue<Vector2>();
            Move(moveAxis.x, moveAxis.z, moveAxis.y);
        }

        private void OnTurn(InputAction.CallbackContext obj)
        {
            turningAxis = obj.action.ReadValue<Vector2>();
            Turning();
        }



        public void Move(float x, float y, float z)
        {

            Vector3 direction = new Vector3(x, y, z);
            Vector3 headRotation = new Vector3(0, head.transform.eulerAngles.y, 0);

            direction = Quaternion.Euler(headRotation) * direction;

            currentGravity = Physics.gravity.y * gravity;

            if (controller.isGrounded)
                currentGravity = 0;

            controller.Move(new Vector3(direction.x * speed, direction.y * speed + currentGravity, direction.z * speed) * Time.deltaTime);
        }

        void Turning()
        {
            //Snap turning
            if (snapTurning)
            {
                transform.rotation *= Quaternion.Euler(0, turnAngle * Math.Sign(turningAxis.x), 0);

                //if (turningAxis.x > 0.7f && axisReset)
                //{
                //    transform.rotation *= Quaternion.Euler(0, turnAngle, 0);
                //    axisReset = false;
                //}
                //else if (turningAxis.x < -0.7f && axisReset)
                //{
                //    transform.rotation *= Quaternion.Euler(0, -turnAngle, 0);
                //    axisReset = false;
                //}

                //if (Mathf.Abs(turningAxis.x) < 0.4f)
                //    axisReset = true;
            }

            //Smooth turning
            else
            {
                transform.rotation *= Quaternion.Euler(0, Time.deltaTime * turnAngle * turningAxis.x, 0);
            }
        }
    }
}
