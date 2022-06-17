using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head_IK : MonoBehaviour
{
    [SerializeField] private Transform rootObject, followObject;
    [SerializeField] private Vector3 positionOffset, rotationOffset, headBodyOffset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        rootObject.position = transform.position + headBodyOffset;
        rootObject.forward = Vector3.ProjectOnPlane(followObject.forward, Vector3.up).normalized;

        transform.position = followObject.TransformPoint(positionOffset);
        transform.rotation = followObject.rotation * Quaternion.Euler(rotationOffset);
    }
}
