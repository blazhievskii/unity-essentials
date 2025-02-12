using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Transform spawnPoint;                 // Точка появления игрока
    public AudioClip checkpointSound;            // Звук активации чекпоинта
    public GameObject effectPrefab;              // Префаб эффекта, который создаётся после активации
    private AudioSource audioSource;             // Аудиоисточник для воспроизведения звука
    private bool activated = false;              // Флаг для проверки активации

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !activated)  // Срабатывает, если чекпоинт ещё не активирован
        {
            ActivateCheckpoint(other.gameObject);
        }
    }

    private void ActivateCheckpoint(GameObject player)
    {
        activated = true;  // Помечаем чекпоинт как активированный

        // Проигрываем звук активации
        if (checkpointSound != null)
        {
            audioSource.PlayOneShot(checkpointSound);
        }

        // Создаём эффект в позиции чекпоинта
        if (effectPrefab != null)
        {
            Instantiate(effectPrefab, transform.position, Quaternion.identity);
        }

        // Устанавливаем чекпоинт для игрока
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.SetCheckpoint(spawnPoint.position);
            Debug.Log("Чекпоинт активирован! Позиция: " + spawnPoint.position);
        }
    }
}