using UnityEngine;

public class RampLauncher : MonoBehaviour
{
    public Vector3 launchDirection = Vector3.forward;  // Направление запуска (настраивается в инспекторе)
    public float launchForce = 20f;  // Сила вылета

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Сбрасываем текущую скорость и запускаем персонажа в заданном направлении
            rb.linearVelocity = Vector3.zero;
            Vector3 force = transform.TransformDirection(launchDirection.normalized) * launchForce;
            rb.AddForce(force, ForceMode.Impulse);
            Debug.Log("🚀 Персонаж запущен в направлении: " + force);
        }
    }

    private void OnDrawGizmos()
    {
        // Визуализация направления запуска в редакторе
        Gizmos.color = Color.red;
        Vector3 direction = transform.TransformDirection(launchDirection.normalized);
        Gizmos.DrawRay(transform.position, direction * 3f);
    }
}