using System;

using UnityEngine;

using Normal.Realtime;
using com.DU.CE.USER;


namespace com.DU.CE.NET
{
    [RequireComponent(typeof(USER_LocalUser))]
    public class NET_LocalAvatar : RealtimeComponent<NCM_UserProperties>
    {
        public EUSERROLE UserRole { get => m_localAvatar.Role; }
      
        private USER_LocalUser m_localAvatar = null;
        private SOC_User m_UserSock = null;

        private void Awake()
        {
            m_localAvatar = GetComponent<USER_LocalUser>();
            m_UserSock = m_localAvatar.UserSock;
        }

        private void OnEnable()
        {
            m_UserSock.OnUserNameChange += SetNetworkPlayerName;
        }

        private void OnDisable()
        {
            m_UserSock.OnUserNameChange += SetNetworkPlayerName;
        }

        protected override void OnRealtimeModelReplaced(NCM_UserProperties previousModel, NCM_UserProperties currentModel)
        {
            //base.OnRealtimeModelReplaced(previousModel, currentModel);
            if (previousModel != null)
            {
                // Unregister from events
                previousModel.userNameDidChange -= OnUserNameChange;
            }

            if (currentModel != null)
            {
                if (currentModel.isFreshModel)
                {
                    SetNetworkPlayerName("--------");
                }

                // Update the mesh render to match the new model
                UpdatePlayerName(currentModel.userName);

                // Register for events so we'll know if the color changes later
                currentModel.userNameDidChange += OnUserNameChange;
                 
                
            }
        }


        internal void SetNetworkPlayerName(string name)
        {
            model.userName = name;
        }

        private void OnUserNameChange(NCM_UserProperties model, string value)
        {
            UpdatePlayerName(model.userName);
        }

        private void UpdatePlayerName(string name)
        {
            m_localAvatar.UpdatePlayerName(name);
        }
    }
}
