using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.DU.CE.AI
{
    [CreateAssetMenu(fileName = "Sock_AI", menuName = "Socks/AI")]
    public class SOC_AI : ScriptableObject
    {
        public int NoOfAI = 0;

        public AI_Avatar[] ais;

        #region LVL_AIActivator Methods ------------------------------------

        internal event Action<ETEAM, Transform> OnActivateAI;
        public void UIActivateHomeAI(Transform location)
        {
            OnActivateAI?.Invoke(ETEAM.HOME, location);
        }
        public void UIActivateAwayAI(Transform location)
        {
            OnActivateAI?.Invoke(ETEAM.AWAY, location);
        }

        #endregion ------------------------------------
    }
}