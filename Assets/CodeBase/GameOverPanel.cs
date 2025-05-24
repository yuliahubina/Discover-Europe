using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverPanel : MonoBehaviour
{
  [SerializeField] private GameObject panel;
  [SerializeField] private Image[] starImages;
  [SerializeField] private Sprite emptyStar;
  [SerializeField] private Sprite filledStar;
  [SerializeField] private TMP_Text timeText;
  [SerializeField] private TMP_Text pointsText;

  private void Awake()
  {
    if (panel != null)
      panel.SetActive(false);
  }

  public void ShowPanel(int lives, float elapsedTime, int points)
  {
    for (int i = 0; i < starImages.Length; i++)
    {
      if (i < lives)
        starImages[i].sprite = filledStar;
      else
        starImages[i].sprite = emptyStar;
    }

    if (timeText != null)
      timeText.text = $"{Mathf.CeilToInt(elapsedTime)}s";

    if (pointsText != null)
      pointsText.text = $"{points}";

    if (panel != null)
      panel.SetActive(true);
  }
}