using UnityEngine;
using UnityEngine.UI;

public class LeaderboardEntryUI : MonoBehaviour
{
    public Text rankText;
    public Text nameText;
    public Text scoreText;

    public void Setup(int rank, string name, int score)
    {
        if (rankText != null) rankText.text = rank.ToString();
        if (nameText != null) nameText.text = name;
        if (scoreText != null)
        {
            scoreText.text = GetFormattedTime(score);
        }
    }
    private string GetFormattedTime(int i)
    {
        int hours = Mathf.FloorToInt(i / 3600f); // Количество часов
        int minutes = Mathf.FloorToInt((i % 3600) / 60f); // Оставшиеся минуты
        int seconds = Mathf.FloorToInt(i % 60f); // Оставшиеся секунды
        return $"{hours:00}:{minutes:00}:{seconds:00}"; // Формат времени "HH:MM:SS"
    }
}
