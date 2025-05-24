using UnityEngine;

public class AchievementManager : MonoBehaviour
{
  [SerializeField] private Achievement[] achievements;
  [SerializeField] private GameObject achievementPrefab;
  [SerializeField] private Transform achievementParent;

  public void UnlockAchievement(string title)
  {
    foreach (var achievement in achievements)
    {
      if (achievement.title == title && !achievement.isUnlocked)
      {
        achievement.isUnlocked = true;
        DisplayAchievement(achievement);
      }
    }
  }

  private void DisplayAchievement(Achievement achievement)
  {
    GameObject achievementObj = Instantiate(achievementPrefab, achievementParent);
    AchievementUI achievementUI = achievementObj.GetComponent<AchievementUI>();
    if (achievementUI != null)
    {
      achievementUI.SetAchievementData(achievement.title, achievement.badgeImage);
    }
  }
}