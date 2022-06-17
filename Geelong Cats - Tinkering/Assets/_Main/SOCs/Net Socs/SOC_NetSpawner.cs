using UnityEngine;
using com.DU.CE.NET;
using System;
using com.DU.CE.NET.NCM;

[CreateAssetMenu(menuName = "NetSocks/SpawnnerConnector")]
public class SOC_NetSpawner : ScriptableObject
{
    private NET_Spawner m_spawner;

    public void NETInititalize(NET_Spawner NetSpawnerRef)
    {
        m_spawner = NetSpawnerRef;
    }

    public GameObject InstantiateNetObject(string objectName, bool isOwnedByClient, bool preventOwnershipTakeover)
    {
        return m_spawner.InstantiateRealTimeView(objectName, isOwnedByClient, preventOwnershipTakeover);
    }

    public GameObject InstantiateNetObject(string objectName, Vector3 pos, Quaternion rot, bool isOwnedByClient, bool preventOwnershipTakeover)
    {
        return m_spawner.InstantiateRealTimeView(objectName, pos, rot, isOwnedByClient, preventOwnershipTakeover);
    }


    public void DestroyNetObject(GameObject _objectToDestroy)
    {
        m_spawner.DestroyRealtimeView(_objectToDestroy);
    }

    internal event Action<NET_BrushStroke> OnBrushStrokeAdded;
    internal void AddBoardBrushStroke(NET_BrushStroke _brushStrokeInfo)
    {
        OnBrushStrokeAdded?.Invoke(_brushStrokeInfo);
    }

    internal event Action OnUndoBoardBrushStroke;
    public void UIUndoBoardBrushStroke()
    {
        OnUndoBoardBrushStroke?.Invoke();
    }

    internal event Action OnClearBoardBrushStrokes;
    public void UIClearBoardStrokes()
    {
        OnClearBoardBrushStrokes?.Invoke();
    }
}