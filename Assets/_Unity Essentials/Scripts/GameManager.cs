using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private int collectedItems = 0; // Количество собранных предметов

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
        // Загружаем количество собранных предметов
        collectedItems = PlayerPrefs.GetInt("CollectedItems_" + SceneManager.GetActiveScene().name, 0);
    }

    private void Update()
    {
        // Если нажата клавиша R — перезапускаем уровень
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }
        
        if (Input.GetKeyDown(KeyCode.T)) 
        {
            ResetProgress();
        }
    }

    public void CollectItem(GameObject item)
    {
        collectedItems++;
        PlayerPrefs.SetInt("CollectedItems_" + SceneManager.GetActiveScene().name, collectedItems);
        PlayerPrefs.SetInt("ItemCollected_" + item.name, 1);
        PlayerPrefs.Save();

        Destroy(item);
    }

    public int GetCollectedItems()
    {
        return collectedItems;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll(); // Удаляем все сохранённые данные
        PlayerPrefs.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Перезапускаем уровень
    }

}