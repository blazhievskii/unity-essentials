using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    
    // Это поле нужно, чтобы вывести финальное время на большой текст 
    [SerializeField] private TextMeshProUGUI finalTimeText;

    private float elapsedTime = 0f;
    private bool isRunning = false;

    void Start()
    {
        // Как только сцена запущена, включаем счёт
        isRunning = true;

        // На всякий случай спрячем финальный текст, если он вдруг активен
        if (finalTimeText != null)
            finalTimeText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    private void UpdateTimerDisplay()
    {
        // Для наглядности отобразим время в формате МИН : СЕК
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public void StopTimer()
    {
        // 1) Останавливаем счёт таймера
        isRunning = false;
    
        // 2) Ставим игру на паузу
        Time.timeScale = 0f;

        // 3) (Опционально) Ставим музыку на паузу
        AudioListener.pause = true;

        // 4) Показываем финальное время
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
    
        if (finalTimeText != null)
        {
            finalTimeText.gameObject.SetActive(true);
            finalTimeText.text = $"Congratulations!\nYour time: {minutes:00}:{seconds:00}\\n\\nShare your time in the comments if you’ve beaten the previous record =)";
            Debug.Log("FinalTimeText Activated");
        }

        if (timerText != null)
        {
            timerText.gameObject.SetActive(false);
        }
    }

}