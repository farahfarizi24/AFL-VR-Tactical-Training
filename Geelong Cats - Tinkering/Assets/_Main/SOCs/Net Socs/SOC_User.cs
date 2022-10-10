using System;
using UnityEngine;

using TMPro;


namespace com.DU.CE.USER
{
    [CreateAssetMenu(menuName = "Socks/User")]
    public class SOC_User : ScriptableObject
    {
        [SerializeField] private SOC_AUserUI m_UserUISock;
        public GameObject CoachInterface;
        public SOC_AUserUI UISock
        {
            get
            {
                if(UserRole == EUSERROLE.COACH)
                {
                    return m_UserUISock as SOC_UserUICoach;
                }
                else
                {
                    return m_UserUISock as SOC_UserUIPlayer;
                }
            }
        }


        [SerializeField] private EUSERROLE m_UserRole;
        public EUSERROLE UserRole { get => m_UserRole; }


        public float m_IKScaleMLP = 1f;



        public event Action<string> OnUserNameChange;
        // Called by button
        public void UIChangeUserName(TextMeshProUGUI _nameField)
        {
            OnUserNameChange?.Invoke(_nameField.text);
            Debug.Log("Field name entered");
            if (UserRole == EUSERROLE.COACH)
            {
                CoachInterface = Resources.Load("UI_ForCoach") as GameObject;
                Instantiate(CoachInterface, new Vector3(0, 1.5f, 0.91f), Quaternion.identity);
                //GameObject CoachPosition = tag
            }
            
        }


        #region UI Methods----------------------------------------------------------

        public event Action<Vector3, Quaternion> OnRequestTeleport;
        internal void RequestRigTeleport(Vector3 _position, Quaternion _rotation)
        {
            Debug.Log("#Eureka#-------------------------" + _position);
            OnRequestTeleport?.Invoke(_position, _rotation);
        }

        #endregion -----------------------------------------------------------------
    }
}