using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AI_BallKicker : MonoBehaviour
{
    private GameObject ball;
    public GameObject cursorPrefab;
    public InputActionReference kickref;
    public Transform ShootPoint;
    public LayerMask layer;
    public LineRenderer lineVisual;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
