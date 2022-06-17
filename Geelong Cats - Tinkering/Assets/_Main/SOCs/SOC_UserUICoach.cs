using System;
using UnityEngine;

namespace com.DU.CE.USER
{
    [CreateAssetMenu(fileName = "Sock_CoachUI", menuName = "Socks/CoachUI")]
    public class SOC_UserUICoach : SOC_AUserUI
    {
        internal event Action<bool, Vector3> OnFieldLocUIToggle;
        public void ToggleFieldLocUI(bool _toggle, Vector3 _position)
        {
            OnFieldLocUIToggle?.Invoke(_toggle, _position);
        }


        public event Action<Transform> OnButtonRugbyBall;
        public void UIButtonRugbyBall(Transform _transform)
        {
            OnButtonRugbyBall?.Invoke(_transform);
        }


        public event Action<bool> OnToggleBallTrail;
        public void UIToggleBallTrail(bool _toggle)
        {
            OnToggleBallTrail?.Invoke(_toggle);
        }
    }
}