using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace com.DU.CE.USER
{
    public class USER_InputManager : MonoBehaviour
    {
        [SerializeField] private SOC_UserStateMachine m_UserStateMachineSock;

        [SerializeField] private InputActionAsset m_DefaultInputAction;

        private void Awake()
        {
            m_DefaultInputAction?.Enable();
        }
    }
}
