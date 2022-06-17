using UnityEngine;
using TMPro;

using com.DU.CE.NET;
using com.DU.CE.USER;
using com.DU.CE.AI;

public class TestConnect : MonoBehaviour
{
    public TextMeshProUGUI RoomName = null;
    public SOC_StadiumProperties StadiumProperties = null;
    public SOC_NetManager NetManager = null;
    public SOC_AI AISock = null;

    [Space]
    public SOC_UserUIPlayer m_playerUI = null;
    public SOC_UserUICoach m_coachUI = null;

    [Space]
    public Transform TestLocation = null;

    public void ConnectAsCoach()
    {
        NetManager.SetNCLocalAvatarPrefab(0);
        NetManager.ConnectToTRoom(RoomName);
    }
    public void ConnectAsPlayer()
    {
        NetManager.SetNCLocalAvatarPrefab(1);
        NetManager.ConnectToTRoom(RoomName);
    }

    public void ToggleBoard(bool _toggle)
    {
        StadiumProperties.ToggleBoard(_toggle);
    }

    public void ActivateAI(ETEAM team)
    {
        if (team.Equals(ETEAM.HOME))
            AISock.UIActivateHomeAI(TestLocation);
        else
            AISock.UIActivateAwayAI(TestLocation);
    }

    public void TeleportToCoach()
    {
        m_playerUI.UITeleportToCoach();
    }

    public void RescaleIKRig()
    {
        if (NetManager.LocalAvatar.GetComponent<USER_LocalUser>().Role == EUSERROLE.COACH)
            m_coachUI.UIRescaleRig();
        else
            m_playerUI.UIRescaleRig();
    }
}
