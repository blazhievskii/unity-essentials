using UnityEngine;

public class VacuumRobot : MonoBehaviour
{
    public float speed = 2f; // Скорость движения
    public float detectionDistance = 0.5f; // Дистанция обнаружения препятствия
    public LayerMask obstacleLayer; // Слой препятствий

    private Vector3 moveDirection = Vector3.forward; // Направление движения

    void Update()
    {
        // Перемещение вперёд
        transform.position += transform.forward * speed * Time.deltaTime;

        // Проверяем, есть ли препятствие впереди
        if (Physics.Raycast(transform.position, transform.forward, detectionDistance, obstacleLayer))
        {
            Rotate(); // Разворачиваемся
        }
    }

    void Rotate()
    {
        transform.Rotate(0f, 180f, 0f); // Разворачиваем робота
    }

    private void OnDrawGizmos()
    {
        // Визуализация луча, который проверяет препятствия
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * detectionDistance);
    }
}