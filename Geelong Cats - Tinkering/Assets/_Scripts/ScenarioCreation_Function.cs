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
    private int AwayAICounter=0;
    private int HomeAICounter =0;

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
        AwayAICounter++;
        Vector3 temp = new Vector3(AwayAICounter,AwayTeamLocation.position.y,AwayTeamLocation.position.z);
        AwayTeamLocation.position = temp;
       AISock.UIActivateAwayAI(AwayTeamLocation);
    }

    private void CreateHomeAI()
    {
        HomeAICounter++;
        Vector3 temp = new Vector3(HomeAICounter, HomeTeamLocation.position.y, HomeTeamLocation.position.z);
        HomeTeamLocation.position = temp;
        AISock.UIActivateHomeAI(HomeTeamLocation);
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
