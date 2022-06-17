using System;
using UnityEngine;

namespace com.DU.CE.USER
{
    [CreateAssetMenu(fileName = "Sock_PlayerUI", menuName = "Socks/PlayerUI")]
    public class SOC_UserUIPlayer : SOC_AUserUI
    {
        internal event Action OnTeleportToCoach;
        public void UITeleportToCoach()
        {
            Debug.Log("#SOC_AUserUI#------------------Teleport To Coach");
            OnTeleportToCoach?.Invoke();
        }
    }
}