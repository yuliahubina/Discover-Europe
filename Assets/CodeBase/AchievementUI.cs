using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementUI : MonoBehaviour
{
  [SerializeField] private TextMeshProUGUI titleText;
  [SerializeField] private Image badgeImage;

  public void SetAchievementData(string title, Sprite badge)
  {
    titleText.text = title;
    badgeImage.sprite = badge;
  }
}