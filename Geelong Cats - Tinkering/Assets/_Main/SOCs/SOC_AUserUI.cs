using System;
using UnityEngine;

namespace com.DU.CE.USER
{
    public abstract class SOC_AUserUI : ScriptableObject
    {
        private const int HandLayer = 22;
        public int HANDLAYER { get => HandLayer; }


        internal event Action<bool> OnHandUIToggle;
        public void ToggleHandUI(bool _toggle)
        {
            //Debug.Log("#SOC_AUserUI#------------------ToggleHandUI " + _toggle);
            OnHandUIToggle?.Invoke(_toggle);
        }


        internal event Action<bool, Vector3> OnBoardLocUIToggle;
        public void ToggleBoardLocUI(bool _toggle, Vector3 _position)
        {
            //Debug.Log("#SOC_AUserUI#------------------ToggleBoardUI " + _toggle);
            OnBoardLocUIToggle?.Invoke(_toggle, _position);
        }


        internal event Action OnUIRescaleRig;
        public void UIRescaleRig()
        {
            OnUIRescaleRig?.Invoke();
        }
    }
}