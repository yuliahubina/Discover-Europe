using System;

[Serializable]
public class LeaderboardEntry
{
  public string playerName;
  public int score;
  public float time;

  public LeaderboardEntry(string name, int score, float time)
  {
    this.playerName = name;
    this.score = score;
    this.time = time;
  }
}