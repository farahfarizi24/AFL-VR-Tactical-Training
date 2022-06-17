using System;
using UnityEngine;

namespace com.DU.CE.LVL
{
    [CreateAssetMenu(menuName = "StateMachineSocks/LvlStateMachine")]
    public class SOC_LvlStateMachine : ScriptableObject
    {
        public ELVLSTATE CurrentState { get; private set; }

        public event Action<ELVLSTATE, ELVLSTATE> OnStateChange;

        public void OnRequestStateChange(ELVLSTATE _StateToChangeTo)
        {
            if (CurrentState == _StateToChangeTo)
                return;

            Debug.LogFormat("#LVLState#----------- LvlState changed from {0} to {1}", CurrentState, _StateToChangeTo);

            OnStateChange?.Invoke(CurrentState, _StateToChangeTo);
            CurrentState = _StateToChangeTo;
        }
    }
}