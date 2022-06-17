using System;
using UnityEngine;

namespace com.DU.CE.USER
{
    [CreateAssetMenu(menuName = "StateMachineSocks/UserStateMachine")]
    public class SOC_UserStateMachine : ScriptableObject
    {
        public EUSERROLE Role { get; private set; }
        internal void SetPlayerRole(EUSERROLE _role)
        {
            Role = _role;
        }

        public event Action<EUSERSTATE, EUSERSTATE> OnStateChange;
        public EUSERSTATE CurrentState { get; private set; }

        public event Action<HandInfo> OnHandStateChange;
        public HandInfo CurrentHandInfo;

        private void OnEnable()
        {
            CurrentState = EUSERSTATE.BLANK;

            CurrentHandInfo.InteractorState = EINTERACTIONSTATE.Blank;
            CurrentHandInfo.Hand = EUSERHAND.NONE;
        }

        public void OnRequestStateChange(EUSERSTATE _StateToChangeTo)
        {
            //Debug.LogFormat("#PlayerState#----------- PlayerState requested to {0}", _StateToChangeTo);

            if (_StateToChangeTo == CurrentState)
                return;

            //Debug.LogFormat("#PlayerState#----------- PlayerState changed from {0} to {1}", CurrentState, _StateToChangeTo);

            OnStateChange?.Invoke(CurrentState, _StateToChangeTo);
            CurrentState = _StateToChangeTo;
        }

        public void RequestInteractionChange(EINTERACTIONSTATE _StateToChangeTo, EUSERHAND _hand)
        {
            //Debug.LogFormat("#HandState#----------- InteractionState of {0} changed to {1}", _hand, _StateToChangeTo);

            switch (_StateToChangeTo)
            {
                case EINTERACTIONSTATE.ONSELECT:

                    HandGrab(_hand);

                    break;

                case EINTERACTIONSTATE.ONGRAB:

                    HandGrab(_hand);

                    break;

                case EINTERACTIONSTATE.ONRELEASE:

                    HandRelease(_hand);

                    break;

                case EINTERACTIONSTATE.ONDESELECT:

                    HandRelease(_hand);

                    break;
            }
            CurrentHandInfo.InteractorState = _StateToChangeTo;

            OnHandStateChange?.Invoke(CurrentHandInfo);
        }

        private void HandGrab(EUSERHAND _hand)
        {
            //Debug.LogFormat("#HandState#----------- HandGrab {0} from {1}", _hand, CurrentHandInfo.Hand);

            if (CurrentHandInfo.Hand == EUSERHAND.NONE)
            {
                CurrentHandInfo.Hand = _hand;
            }
            else
            {
                CurrentHandInfo.Hand = EUSERHAND.BOTH;
            }
        }

        private void HandRelease(EUSERHAND _hand)
        {
            //Debug.LogFormat("#HandState#----------- HandRelease {0}", _hand);

            if (_hand == CurrentHandInfo.Hand)
            {
                CurrentHandInfo.Hand = EUSERHAND.NONE;
            }
        }
    }


}