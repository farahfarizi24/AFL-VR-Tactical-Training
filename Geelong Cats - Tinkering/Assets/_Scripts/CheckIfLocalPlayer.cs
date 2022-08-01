using Autohand.Demo;
using Normal.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfLocalPlayer : MonoBehaviour
{
    public RealtimeView _realtimeView;

    public GameObject[] gameObjectsToTurnOff;
    public XRHandControllerLink[] hands;
    // Start is called before the first frame update
    void Start()
    {
        if (_realtimeView.isOwnedLocallyInHierarchy)
        {
            Debug.Log("Player is owned locally");
        }
        else
        {
            Debug.Log("Player networked");
            TurnOffLocalOnlyObjects();
        }
    }

    private void TurnOffLocalOnlyObjects()
    {
        foreach (GameObject go in gameObjectsToTurnOff)
        {
            go.SetActive(false);
        }
        foreach (XRHandControllerLink hand in hands)
        {
            hand.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
