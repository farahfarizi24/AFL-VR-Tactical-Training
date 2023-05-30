using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSensor : MonoBehaviour
{
    public bool SensorTrigger;
    // Start is called before the first frame update
    void Start()
    {
        SensorTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)


    {
       if(other.gameObject.CompareTag("BallContainer"))
        {
            SensorTrigger = true;
            Debug.Log("Sensor Trigger Active");
        }
    
    }

    private void OnTriggerExit(Collider other)
    {
        SensorTrigger = false;
    }
}
