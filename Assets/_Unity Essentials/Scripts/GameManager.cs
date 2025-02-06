using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private Vector3 lastCheckpointPosition; // Позиция последнего чекпоинта
    private bool isRestarting = false; // Флаг рестарта

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Проверяем, это рестарт или новая игра
        if (PlayerPrefs.GetInt("IsRestarting", 0) == 1) 
        {
            LoadCheckpoint(); // Загружаем чекпоинт (будет вызван до старта сцены)
        }
    }

    public void RestartLevel()
    {
        PlayerPrefs.SetInt("IsRestarting", 1); // Фиксируем, что это рестарт
        PlayerPrefs.Save();
        SceneManager.sceneLoaded += OnSceneLoaded; // Подписываемся на событие загрузки сцены
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadCheckpoint(); // Перемещаем игрока в чекпоинт сразу после загрузки сцены
        SceneManager.sceneLoaded -= OnSceneLoaded; // Отписываемся от события (чтобы не вызывалось постоянно)
    }

    private void LoadCheckpoint()
    {
        if (PlayerPrefs.HasKey("SpawnX"))
        {
            lastCheckpointPosition = new Vector3(
                PlayerPrefs.GetFloat("SpawnX"),
                PlayerPrefs.GetFloat("SpawnY"),
                PlayerPrefs.GetFloat("SpawnZ")
            );

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = lastCheckpointPosition;
                Debug.Log($"✅ Игрок загружен в чекпоинт: {lastCheckpointPosition}");
            }
        }

        // Сбрасываем флаг рестарта, чтобы после перезапуска игры игрок начинал с начала
        PlayerPrefs.SetInt("IsRestarting", 0);
        PlayerPrefs.Save();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }
    }
}
