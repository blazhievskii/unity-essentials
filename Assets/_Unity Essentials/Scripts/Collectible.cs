using UnityEngine;

public class Collectible : MonoBehaviour
{
   
    public GameObject explosionEffect;
    public AudioClip soundOnCollision;

    private void Start()
    {
        gameObject.SetActive(true); // Всегда включаем предмет при старте
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (explosionEffect != null)
            {
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
            }
            
            if (soundOnCollision != null)
            {
                AudioSource.PlayClipAtPoint(soundOnCollision, transform.position);
            }
            
            Destroy(gameObject);
        }
    }
}