using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Day Length Settings")]
    [Tooltip("Duration of a full day cycle in seconds")]
    public float dayDurationInSeconds = 60f; 

    private float rotationSpeed;

    void Start()
    {
        // Calculate rotation speed based on the day duration
        rotationSpeed = 360f / dayDurationInSeconds;
    }

    void Update()
    {
        // Rotate the directional light around the X-axis
        transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
    }
}