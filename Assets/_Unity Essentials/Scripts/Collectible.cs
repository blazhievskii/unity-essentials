using UnityEngine;

public class Collectible : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(true); // Всегда включаем предмет при старте
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}