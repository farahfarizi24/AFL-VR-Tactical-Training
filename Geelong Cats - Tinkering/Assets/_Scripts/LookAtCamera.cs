using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    void Update()
    {
        Vector3 targetPosition = new Vector3(Camera.main.transform.position.x,
                                               this.transform.position.y,
                                               Camera.main.transform.position.z);
        transform.rotation = Quaternion.LookRotation(transform.position - targetPosition);
    }
}
