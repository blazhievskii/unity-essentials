using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Day Length Settings")] [Tooltip("Duration of a full day cycle in seconds")]
    public float dayDurationInSeconds = 60f;

    private float rotationSpeed;

    void Start()
    {
        rotationSpeed = 360f / dayDurationInSeconds;
    }

    void Update()
    {
        transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
    }
}