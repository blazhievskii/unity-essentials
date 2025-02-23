using UnityEngine;

public class RampLauncher : MonoBehaviour
{
    public Vector3 launchDirection = Vector3.forward;
    public float launchForce = 20f;

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            Vector3 force = transform.TransformDirection(launchDirection.normalized) * launchForce;
            rb.AddForce(force, ForceMode.Impulse);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 direction = transform.TransformDirection(launchDirection.normalized);
        Gizmos.DrawRay(transform.position, direction * 3f);
    }
}