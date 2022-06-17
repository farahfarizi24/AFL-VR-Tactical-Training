using UnityEngine;

using Normal.Realtime;

namespace com.DU.CE.NET
{
    [RequireComponent(typeof(NET_Manager))]
    public class NET_Spawner : MonoBehaviour
    {
        [SerializeField] private SOC_NetSpawner m_netSpawnnerConnector;

        private NET_Manager m_netManager;

        private void Awake()
        {
            m_netManager = GetComponent<NET_Manager>();

            // Add reference to SOC_NetSpawnner
            m_netSpawnnerConnector.NETInititalize(this);
        }

        // This method is used to instantiate anything to the world.
        internal GameObject InstantiateRealTimeView(string objectName, Vector3 pos, Quaternion rot, bool isOwnedByClient, bool preventTakeoverOwnership)
        {
            GameObject temp = Realtime.Instantiate(
                                objectName,
                                position: pos,
                                rotation: rot,
                                ownedByClient: isOwnedByClient,
                                useInstance: m_netManager.NCRealtime);          // Use the instance of Realtime that fired the didConnectToRoom event.

            temp.GetComponent<RealtimeTransform>().RequestOwnership();
            temp.GetComponent<RealtimeView>().preventOwnershipTakeover = preventTakeoverOwnership;

            return temp;
        }

        internal GameObject InstantiateRealTimeView(string objectName, bool isOwnedByClient, bool preventTakeoverOwnership)
        {
            GameObject temp = Realtime.Instantiate(objectName, isOwnedByClient, preventTakeoverOwnership);

            if (isOwnedByClient)
            {
                temp.GetComponent<RealtimeTransform>()?.RequestOwnership();
            }

            return temp;
        }

        internal void DestroyRealtimeView(GameObject _objectToDestroy)
        {
            Realtime.Destroy(_objectToDestroy);
        }
    }
}