using TMPro;
using UnityEngine;

public class LeaderboardEntryUI : MonoBehaviour
{
  [SerializeField] private TextMeshProUGUI positionText;
  [SerializeField] private TextMeshProUGUI nameText;
  [SerializeField] private TextMeshProUGUI scoreText;
  [SerializeField] private TextMeshProUGUI timeText;

  public void SetEntryData(int position, string name, int score, float time)
  {
    positionText.text = position.ToString();
    nameText.text = name;
    scoreText.text = score.ToString();
    timeText.text = FormatTime(time);
  }

  private string FormatTime(float time)
  {
    return $"{Mathf.FloorToInt(time)} sec";
  }
}