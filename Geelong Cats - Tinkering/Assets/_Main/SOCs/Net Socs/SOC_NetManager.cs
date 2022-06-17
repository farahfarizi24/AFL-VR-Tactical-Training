using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;

namespace com.DU.CE.NET
{ 
    [CreateAssetMenu(menuName = "NetSocks/NetManager")]
    public class SOC_NetManager : ANetworkSOC
    {
        public void SetNCLocalAvatarPrefab(int _roleID)
        {
            p_netManager.SetNCLocalAvatarPrefab((EUSERROLE)_roleID);
        }

        public void ConnectToTRoom(TextMeshProUGUI roomName)
        {
            p_netManager.ConnectToRoom(roomName.text);
        }

        public event Action<int, string> OnDisconnected;
        internal void Disconnect(int _messageID, string _message)
        {
            OnDisconnected?.Invoke(_messageID, _message);
        } 


        public Transform LocalAvatar { get; private set; }
        public Transform Coach { get; private set; }
        public List<Transform> Players = new List<Transform>();

        internal void SetLocalAvatar(Transform _localAvatarTransform)
        {
            Debug.Log("#Network Manager#----------Local player created");

            LocalAvatar = _localAvatarTransform;
        }

        internal void SetCoach(Transform _coachTransform)
        {
            Debug.Log("#Network Manager#----------Coach clone created");

            Coach = _coachTransform;
        }

        internal void AddPlayerToList(Transform _playerTransform)
        {
            Debug.Log("#Network Manager#----------Player clone created");

            Players.Add(_playerTransform);
        }


        private void Awake()
        {
            LocalAvatar = null;
            Coach = null;
            Players.Clear();
        }
    }
}