using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayerUIHides : MonoBehaviour
{
    public GameObject CoachPanel=null;
    // Start is called before the first frame update
    void Start()
    {
        CoachPanel = GameObject.FindGameObjectWithTag("CoachUI");
        CoachPanel.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
