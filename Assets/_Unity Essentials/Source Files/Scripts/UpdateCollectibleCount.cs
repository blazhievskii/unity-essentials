using UnityEngine;
using TMPro;
using System; // Для класса Type

public class UpdateCollectibleCount : MonoBehaviour
{
    private TextMeshProUGUI collectibleText;

    // Нужно, чтобы один раз остановить таймер, 
    // и не дергать его повторно в каждом кадре
    private bool timerAlreadyStopped = false;

    void Start()
    {
        collectibleText = GetComponent<TextMeshProUGUI>();
        if (collectibleText == null)
        {
            Debug.LogError("UpdateCollectibleCount script requires a TextMeshProUGUI component on the same GameObject.");
            return;
        }
        UpdateCollectibleDisplay(); // Однократный вызов на старте
    }

    void Update()
    {
        UpdateCollectibleDisplay();
    }

    private void UpdateCollectibleDisplay()
    {
        int totalCollectibles = 0;

        // Проверяем объекты типа Collectible
        Type collectibleType = Type.GetType("Collectible");
        if (collectibleType != null)
        {
            totalCollectibles += UnityEngine.Object.FindObjectsByType(collectibleType, FindObjectsSortMode.None).Length;
        }

        // Если есть Collectible2D, тоже считаем
        Type collectible2DType = Type.GetType("Collectible2D");
        if (collectible2DType != null)
        {
            totalCollectibles += UnityEngine.Object.FindObjectsByType(collectible2DType, FindObjectsSortMode.None).Length;
        }

        // Отображаем текущее число
        collectibleText.text = $"Collectibles remaining: {totalCollectibles}";

        // Если все предметы собраны - останавливаем таймер (один раз)
        if (totalCollectibles == 0 && !timerAlreadyStopped)
        {
            timerAlreadyStopped = true;

            // Ищем в сцене наш скрипт GameTimer и вызываем StopTimer()
            GameTimer timer = FindObjectOfType<GameTimer>();
            if (timer != null)
            {
                timer.StopTimer();
            }
        }
    }
}