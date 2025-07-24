using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float totalTimeInSeconds = 900f; //15 minutes

    private float currentTime;

    private void Update()
    {
        if (currentTime < totalTimeInSeconds)
        {
            currentTime += Time.deltaTime;
            currentTime = Mathf.Max(currentTime, 0f); // Clamp to 0
            UpdateTimerUI();
        }
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}
