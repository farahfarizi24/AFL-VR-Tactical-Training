using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.DU.CE.AI;

public class PlayerToAICollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Home")){
            Debug.Log("PossessionInitiated");

            if (other.gameObject.GetComponent<AI_Avatar>().IsPositionReference == true)
            {
                other.gameObject.GetComponent<AI_Avatar>().AIModel.SetActive(false);
            }
        }
    }
}
