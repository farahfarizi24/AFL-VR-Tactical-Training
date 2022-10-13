using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.DU.CE.AI;

using com.DU.CE.NET;
using com.DU.CE.USER;
using System;

public class ScenarioCreation_Function : MonoBehaviour
{
    public SOC_AI AISock = null;
    public Transform HomeTeamLocation=null;
    public Transform AwayTeamLocation = null;


    /// <summary>
    ///  Under is the button


    public Button HomeCreate;
    public Button AwayCreate;
    // Start is called before the first frame update
    void Start()
    {
        GameObject HomeLoc = GameObject.FindGameObjectWithTag("HomeSpawnLoc");
        GameObject AwayLoc = GameObject.FindGameObjectWithTag("AwaySpawnLoc");
        HomeTeamLocation = HomeLoc.transform;
        AwayTeamLocation = AwayLoc.transform;
        HomeCreate.onClick.AddListener(CreateHomeAI);
        AwayCreate.onClick.AddListener(CreateAwayAI);


       
    }

    private void CreateAwayAI()
    {
        AISock.UIActivateHomeAI(HomeTeamLocation);
    }

    private void CreateHomeAI()
    {
        AISock.UIActivateAwayAI(AwayTeamLocation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  /*  public void ActivateAI(ETEAM team)
    {
        if (team.Equals(ETEAM.HOME))
            AISock.UIActivateHomeAI(HomeTeamLocation);
        else
            AISock.UIActivateAwayAI(AwayTeamLocation);
    }
  */

}
