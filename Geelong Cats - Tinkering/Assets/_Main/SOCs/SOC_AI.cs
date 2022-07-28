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
        internal event Action<ETEAM, int> OnDisableAIByNum;
        internal event Action<ETEAM, int, Transform> OnActivateAIByNum;
        internal event Action<ETEAM, int> OnTeamSizeChanged;



        public void UIActivateHomeAI(Transform location)
        {
            OnActivateAI?.Invoke(ETEAM.HOME, location);
        }
        public void UIActivateAwayAI(Transform location)
        {
            OnActivateAI?.Invoke(ETEAM.AWAY, location);
        }
        public void UIActivateHomeAIByNum(Transform location, int num)
        {
            OnActivateAIByNum?.Invoke(ETEAM.HOME, num, location);
        }
        public void UIActivateAwayAIByNum(Transform location, int num)
        {
            OnActivateAIByNum?.Invoke(ETEAM.AWAY, num, location);
        }
        public void DisableHomeAIByNum(int num)
        {
            OnDisableAIByNum?.Invoke(ETEAM.HOME, num);
        }
        public void DisableAwayAIByNum(int num)
        {
            OnDisableAIByNum?.Invoke(ETEAM.AWAY, num);
        }
        public void ChangeHomeTeamSize(int size)
        {
            OnTeamSizeChanged?.Invoke(ETEAM.HOME, size);
        }
        public void ChangeAwayTeamSize(int size)
        {
            OnTeamSizeChanged?.Invoke(ETEAM.AWAY, size);
        }

        #endregion ------------------------------------
    }
}