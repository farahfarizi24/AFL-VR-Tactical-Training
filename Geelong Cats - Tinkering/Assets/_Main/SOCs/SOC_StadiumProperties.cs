using System.Collections.Generic;
using UnityEngine;

using System;

[CreateAssetMenu(menuName = "NetSocks/StadiumPropertiesSoc")]
public class SOC_StadiumProperties : ScriptableObject
{
    private void OnEnable()
    {
        m_markedPositions.Clear();
    }

    internal event Action OnSetupPlayer;
    internal void SetupPlayer()
    {
        OnSetupPlayer?.Invoke();
    }


    #region FieldLines -----------------------------------------------------------

    public void SetupFieldLines(EUSERROLE RoleToSetupFor)
    {
        if (RoleToSetupFor == EUSERROLE.COACH)
        {
            ChangeNetworkFieldLength(FieldLength);
            ChangeNetworkFieldWidth(FieldWidth);
        }
    }

    [Header("Field Lines Properties")]
    [SerializeField] private int m_FieldLength = 0;
    public int FieldLength
    {
        get { return m_FieldLength; }
        set { m_FieldLength = value; }
    }
    [SerializeField] private int m_FieldWidth = 0;
    public int FieldWidth
    {
        get { return m_FieldWidth; }
        set { m_FieldWidth = value; }
    }

    public event Action<int> OnChangeNetworkFieldLength;
    public event Action<int> OnChangeNetworkFieldWidth;

    public event Action OnNetworkSetFieldDimensions;

    // This is for Coach UI to update the network model directly
    internal void ChangeNetworkFieldLength(int _length)
    {
        m_FieldLength = _length;
        OnChangeNetworkFieldLength?.Invoke(_length);
    }
    internal void ChangeNetworkFieldWidth(int _width)
    {
        m_FieldWidth = _width;
        OnChangeNetworkFieldWidth?.Invoke(_width);
    }
    //-------------

    // This is for network model to update field lines on the player side and coach side
    // This is done automatically for the players when they connect to a room with field lines
    public void NetworkSetFieldLength(int _length)
    {
        m_FieldLength = _length;
        OnNetworkSetFieldDimensions?.Invoke();
    }

    public void NetworkSetFieldWidth(int _width)
    {
        m_FieldWidth = _width;
        OnNetworkSetFieldDimensions?.Invoke();
    }
    //-------------

    #endregion ------------------------------------------------------------



    #region Marker Positions -----------------------------------------------------

    private List<Vector3> m_markedPositions = new List<Vector3>();
    public Vector3[] MarkerPositions 
    {
        get => m_markedPositions.ToArray(); 
    }

    public void AddMarkerToList(Vector3 _localPinPos)
    {
        m_markedPositions.Add(_localPinPos);
    }

    internal event Action<Vector3> RequestMarkerAddToNetwork;
    public void AddNewMarkerToNetwork(Vector3 _localPinPos)
    {
        RequestMarkerAddToNetwork?.Invoke(_localPinPos);
    }

    internal event Action<Vector3> OnNetworkMarkerAdd;
    internal void NetworkAddMarker(Vector3 _localPinPos)
    {
        m_markedPositions.Add(_localPinPos);
        OnNetworkMarkerAdd?.Invoke(_localPinPos);
    }

    #endregion -----------------------------------------------------------------

    internal bool isBoardOpen = false;

    public event Action<bool> OnBoardToggle;
    public event Action<bool> OnNetworkFieldBoardToggle;

    public void ToggleBoard(bool _toggle)
    {
        OnBoardToggle?.Invoke(_toggle);
    }

    public void NetworkBoardToggle(bool _toggle)
    {
        isBoardOpen = _toggle;
        OnNetworkFieldBoardToggle?.Invoke(_toggle);
    }
}

