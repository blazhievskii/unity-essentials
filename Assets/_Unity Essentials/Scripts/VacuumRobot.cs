using UnityEngine;

public class VacuumRobot : MonoBehaviour
{
    public float speed = 2f;
    public float detectionDistance = 0.5f;
    public LayerMask obstacleLayer;

    private Vector3 moveDirection = Vector3.forward;

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;


        if (Physics.Raycast(transform.position, transform.forward, detectionDistance, obstacleLayer))
        {
            Rotate();
        }
    }

    void Rotate()
    {
        transform.Rotate(0f, 180f, 0f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * detectionDistance);
    }
}