using UnityEngine;
using com.DU.CE.NET;

public abstract class ANetworkSOC : ScriptableObject
{
    protected NET_Manager p_netManager;

    public virtual void Inititalize(NET_Manager netManager)
    {
        p_netManager = netManager;

        Debug.Log("Initialising network sock: " + this.name);
    }
}
