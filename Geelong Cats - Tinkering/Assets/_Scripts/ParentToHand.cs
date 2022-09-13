using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentToHand : MonoBehaviour
{
    [SerializeField] private GameObject RightHandController;
    [SerializeField] private GameObject LeftHandController;
    // Start is called before the first frame update
    void Start()
    {
        RightHandController = GameObject.FindGameObjectWithTag("RightHandController");
        LeftHandController = GameObject.FindGameObjectWithTag("LeftHandController");
        SetToLeftHand();
    }

   public void  SetToRightHand()
    {
        Debug.Log("Set To Right Hand");
        transform.position = LeftHandController.transform.position;
    }

  public   void SetToLeftHand()
    {
        Debug.Log("Set To Left Hand");
        transform.position = RightHandController.transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
