using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;


    [SerializeField] private TextMeshProUGUI finalTimeText;

    private float elapsedTime = 0f;
    private bool isRunning = false;

    void Start()
    {
        isRunning = true;


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
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public void StopTimer()
    {
        isRunning = false;

        Time.timeScale = 0f;

        AudioListener.pause = true;

        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        if (finalTimeText != null)
        {
            finalTimeText.gameObject.SetActive(true);
            finalTimeText.text =
                $"Congratulations!\nYour time: {minutes:00}:{seconds:00}\\n\\nShare your time in the comments if you’ve beaten the previous record =)";
        }

        if (timerText != null)
        {
            timerText.gameObject.SetActive(false);
        }
    }
}