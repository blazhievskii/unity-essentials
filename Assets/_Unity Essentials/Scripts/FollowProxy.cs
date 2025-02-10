using UnityEngine;

public class FollowProxy : MonoBehaviour
{
    public Transform target; // Ссылка на персонажа

    void LateUpdate()
    {
        if (target != null)
        {
            // Следим только за позицией персонажа.
            transform.position = target.position;
            // Не копируем вращение — оставляем ориентацию по умолчанию (например, Quaternion.identity)
            // Или, если нужно, можно задать какую-то фиксированную ориентацию.
            float targetYaw = target.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0, targetYaw, 0);
        }
    }
}