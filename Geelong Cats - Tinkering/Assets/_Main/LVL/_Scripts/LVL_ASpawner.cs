using System.Collections;
using UnityEngine;

namespace com.DU.CE.LVL
{
    public abstract class LVL_ASpawner : MonoBehaviour
    {
        [SerializeField] protected SOC_NetSpawner p_NetSpawnerSock;
        [SerializeField] protected SOC_LvlStateMachine p_LevelStateMachineSock;

        private bool m_isInitialised = false;

        protected virtual void OnEnable()
        {
            p_LevelStateMachineSock.OnStateChange += OnLevelStateChange;
        }

        protected virtual void OnDisable()
        {
            p_LevelStateMachineSock.OnStateChange -= OnLevelStateChange;
        }

        protected virtual void OnLevelStateChange(ELVLSTATE _oldState, ELVLSTATE _newState)
        {
            if (_newState == ELVLSTATE.ONCONNECT_COACH)
            {
                if (!m_isInitialised)
                    StartCoroutine(OnCoachSetup());

                m_isInitialised = true;
            }
            else if(_newState == ELVLSTATE.ONCONNECT_PLAYER)
            {
                StartCoroutine(OnPlayerSetup());
            }
        }


        protected abstract IEnumerator OnCoachSetup();
        protected abstract IEnumerator OnPlayerSetup();
    }
}