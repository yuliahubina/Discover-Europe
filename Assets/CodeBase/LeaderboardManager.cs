using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
  [SerializeField] private GameObject playerEntryPrefab;
  [SerializeField] private Transform leaderboardContent;
  [SerializeField] private int numberOfBots = 7;

  [Header("Score Range")] 
  [SerializeField] private int minScore = 100;
  [SerializeField] private int maxScore = 1000;

  [Header("Time Range")] 
  [SerializeField] private float minTime = 60;
  [SerializeField] private float maxTime = 600;

  private List<LeaderboardEntry> _leaderboardEntries = new();

  private void Start()
  {
    GenerateBotEntries();
  }

  public void AddPlayerEntry(string playerName, int score, float time)
  {
    _leaderboardEntries.Add(new LeaderboardEntry(playerName, score, time));
    SortLeaderboard();
    DisplayLeaderboard();
  }

  private void GenerateBotEntries()
  {
    string[] botNames = Enum.GetNames(typeof(BotNames));
    for (int i = 0; i < numberOfBots; i++)
    {
      string randomName = botNames[UnityEngine.Random.Range(0, botNames.Length)];
      int randomScore = UnityEngine.Random.Range(minScore, maxScore);
      float randomTime = UnityEngine.Random.Range(minTime, maxTime);
      _leaderboardEntries.Add(new LeaderboardEntry(randomName, randomScore, randomTime));
    }

    SortLeaderboard();
    DisplayLeaderboard();
  }

  private void SortLeaderboard()
  {
    _leaderboardEntries = _leaderboardEntries.OrderByDescending(entry => entry.score)
      .ThenBy(entry => entry.time)
      .ToList();
  }

  private void DisplayLeaderboard()
  {
    foreach (Transform child in leaderboardContent)
    {
      Destroy(child.gameObject);
    }

    for (int i = 0; i < _leaderboardEntries.Count; i++)
    {
      GameObject entry = Instantiate(playerEntryPrefab, leaderboardContent);
      LeaderboardEntryUI entryUI = entry.GetComponent<LeaderboardEntryUI>();
      if (entryUI != null)
      {
        entryUI.SetEntryData(i + 1, _leaderboardEntries[i].playerName, _leaderboardEntries[i].score,
          _leaderboardEntries[i].time);
      }
    }
  }
}

public enum BotNames
{
  John,
  Alice,
  Bob,
  Charlie,
  David,
  Eve,
  Frank,
  Grace,
  Hank,
  Irene
}