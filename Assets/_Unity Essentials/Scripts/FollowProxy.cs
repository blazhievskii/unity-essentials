using UnityEngine;

public class FollowProxy : MonoBehaviour
{
    public Transform target;

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position;

            float targetYaw = target.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0, targetYaw, 0);
        }
    }
}