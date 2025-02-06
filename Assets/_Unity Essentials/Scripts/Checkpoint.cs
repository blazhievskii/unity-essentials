using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Transform spawnPoint; // Точка появления игрока

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Обнаружено столкновение с: " + other.gameObject.name);

        if (other.CompareTag("Player"))
        {
            PlayerPrefs.SetFloat("SpawnX", spawnPoint.position.x);
            PlayerPrefs.SetFloat("SpawnY", spawnPoint.position.y);
            PlayerPrefs.SetFloat("SpawnZ", spawnPoint.position.z);
            PlayerPrefs.Save();

            Debug.Log("Чекпоинт сохранён! Позиция: " + spawnPoint.position);
        }
    }
}