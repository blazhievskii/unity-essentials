using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Сюда мы назначим CameraAnchor (не сам персонаж!)
    public Vector3 offset = new Vector3(0, 3, -5); // Смещение камеры относительно персонажа
    public float smoothSpeed = 5f; // Скорость сглаживания движения камеры

    private void LateUpdate()
    {
        if (target == null) return;

        // Берем только Y-поворот персонажа, чтобы избежать переворачивания
        Quaternion targetRotation = Quaternion.Euler(0, target.eulerAngles.y, 0);

        // Вычисляем позицию камеры с учётом смещения
        Vector3 desiredPosition = target.position + targetRotation * offset;

        // Плавное движение камеры
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Камера всегда смотрит на игрока
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}