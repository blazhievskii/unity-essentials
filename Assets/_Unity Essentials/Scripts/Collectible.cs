using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    
    public float rotationSpeed;
    public GameObject onCollectEffect;
    
    private void Start()
    {
        // Проверяем, был ли предмет уже собран
        if (PlayerPrefs.GetInt("ItemCollected_" + gameObject.name, 0) == 1)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        transform.Rotate(0,rotationSpeed, 0);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.CollectItem(gameObject);
            Instantiate(onCollectEffect, transform.position, transform.rotation);
        }
    }
}

