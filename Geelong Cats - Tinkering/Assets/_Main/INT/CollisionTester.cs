using UnityEngine;

public class CollisionTester : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("#CollisionTester#-------------------------" + collision.collider.name);
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("#CollisionTester#-------------------------" + collision.collider.name);

    }
}
