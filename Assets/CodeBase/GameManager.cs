using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class GameManager : MonoBehaviour
{
  #region UI References

  [Header("UI References")] 
  [SerializeField] private TMP_Text targetCountryText;
  [SerializeField] private TMP_Text timerText;
  [SerializeField] private TMP_Text pointsText;
  [SerializeField] private GameOverPanel gameOverPanel;
  [SerializeField] private GameObject mainPanel;
  [SerializeField] private GameObject difficultyPanel;

  #endregion

  #region Leaderboard

  [Header("Leaderboard")] 
  [SerializeField] private LeaderboardManager leaderboardManager;
  [SerializeField] private GameObject leaderboardPanel;

  #endregion

  #region Achievements

  [Header("Achievements")] 
  [SerializeField] private AchievementManager achievementManager;
  [SerializeField] private int europeExplorerThreshold = 20;
  [SerializeField] private int quizChampionThreshold = 10;
  [SerializeField] private int speedRunnerThreshold = 20;

  #endregion

  #region Lives UI References
  
  [Header("Lives UI References")] 
  [SerializeField] private Image[] starImages;
  [SerializeField] private Sprite emptyStar;
  [SerializeField] private Sprite filledStar;
  
  #endregion

  #region Game Settings

  [Header("Game Settings")] 
  [SerializeField] private float roundDelay = 1f;
  [SerializeField] private AudioClip bonusSound;
  [SerializeField] private AudioSource globalAudioSource;
  [SerializeField] private int lives = 3;
  [SerializeField] private float gameDuration = 60f;

  #endregion

  #region StoryMode

  [Header("Story Screens")] 
  [SerializeField] private GameObject firstScreen;
  [SerializeField] private GameObject secondScreen;
  [SerializeField] private GameObject thirdScreen;

  #endregion

  private List<Country> _allCountries;
  private Country _currentCountry;
  private List<Country> _availableCountries = new List<Country>();
  private bool _isInputBlocked;
  private float _timeRemaining;
  private bool _isGameOver = false;
  private int _points = 0;
  private int _correctAnswersInARow = 0;
  private int _correctAnswersCount = 0;

  #region Life Sycle

  private void Awake()
  {
    _allCountries = FindObjectsOfType<Country>().ToList();

    foreach (var country in _allCountries)
    {
      Country localCountry = country;
      localCountry.OnCountrySelected.AddListener(() => HandleCountrySelection(localCountry));
    }
  }

  private void Start()
  {
    ShowFirstScreen();
    InitializeGame();
    _timeRemaining = gameDuration;
    UpdateTimerDisplay();
    UpdatePointsDisplay();
    UpdateLivesDisplay();
  }

  private void Update()
  {
    if (!_isGameOver)
    {
      _timeRemaining -= Time.deltaTime;
      UpdateTimerDisplay();
      if (_timeRemaining <= 0f)
      {
        EndGame();
      }
    }
  }

  private void InitializeGame()
  {
    _availableCountries = new List<Country>(_allCountries);
    lives = 3;
    _points = 0;
    _isGameOver = false;
    SelectNewCountry();
  }
  
  #endregion

  private void UpdateTimerDisplay()
  {
    if (timerText != null)
      timerText.text = $"{Mathf.CeilToInt(_timeRemaining)}";
  }

  private void UpdatePointsDisplay()
  {
    if (pointsText != null)
      pointsText.text = $"{_points}";
  }

  private void UpdateLivesDisplay()
  {
    for (int i = 0; i < starImages.Length; i++)
    {
      starImages[i].sprite = (i < lives) ? filledStar : emptyStar;
    }
  }

  private void SelectNewCountry()
  {
    if (_availableCountries.Count == 0)
    {
      EndGame();
      return;
    }

    _currentCountry = _availableCountries[Random.Range(0, _availableCountries.Count)];
    _availableCountries.Remove(_currentCountry);

    UpdateTargetDisplay();
    _isInputBlocked = false;
  }

  private void UpdateTargetDisplay()
  {
    if (targetCountryText != null)
      targetCountryText.text = $"Find: {_currentCountry.CountryName}";
  }

  public void OnSkipCountryButtonClick()
  {
    SelectNewCountry();
  }

  private void HandleCountrySelection(Country selectedCountry)
  {
    if (_isInputBlocked) return;
    _isInputBlocked = true;

    if (selectedCountry == _currentCountry)
    {
      selectedCountry.HandleCorrectAnswer(_correctAnswersInARow >= 3);
      selectedCountry.HideCountry();

      if (globalAudioSource != null)
        globalAudioSource.Play();

      _correctAnswersCount++;
      _correctAnswersInARow++;

      if (_correctAnswersCount == 15)
      {
        ShowSecondScreen();
        SelectNewCountry();
        return;
      }

      if (_correctAnswersCount == 39)
      {
        ShowThirdScreen();
        SelectNewCountry();
        return;
      }

      if (_correctAnswersCount == europeExplorerThreshold)
      {
        difficultyPanel.SetActive(true);
      }

      if (_correctAnswersCount >= europeExplorerThreshold)
      {
        achievementManager.UnlockAchievement("Europe Explorer");
      }

      if (_correctAnswersInARow >= quizChampionThreshold)
      {
        achievementManager.UnlockAchievement("Quiz Champion");
      }

      if (_correctAnswersInARow >= 3)
      {
        _points += 200;
        if (bonusSound != null && globalAudioSource != null)
          globalAudioSource.PlayOneShot(bonusSound);
      }
      else
      {
        _points += 100;
      }

      UpdatePointsDisplay();
      Invoke(nameof(SelectNewCountry), roundDelay);
    }
    else
    {
      selectedCountry.HandleWrongAnswer(_currentCountry.CountryName);
      lives--;
      UpdateLivesDisplay();
      _correctAnswersInARow = 0;
      if (lives <= 0)
      {
        EndGame();
      }
      else
      {
        _isInputBlocked = false;
      }
    }
  }

  #region Story Mode

  private void ShowFirstScreen()
  {
    firstScreen.SetActive(true);
    Time.timeScale = 0;
  }

  public void OnFirstScreenContinueButton()
  {
    firstScreen.SetActive(false);
    Time.timeScale = 1;
  }

  private void ShowSecondScreen()
  {
    secondScreen.SetActive(true);
    Time.timeScale = 0;
  }

  public void OnSecondScreenContinueButton()
  {
    secondScreen.SetActive(false);
    Time.timeScale = 1;
  }

  private void ShowThirdScreen()
  {
    thirdScreen.SetActive(true);
    Time.timeScale = 0;
  }

  public void OnThirdScreenContinueButton()
  {
    thirdScreen.SetActive(false);
    Time.timeScale = 1;
  }

  #endregion

  private void EndGame()
  {
    float elapsedTime = gameDuration - (_timeRemaining > 0 ? _timeRemaining : 0);
    int additionalPoints = Mathf.CeilToInt(_timeRemaining);
    _points += additionalPoints;

    if (_timeRemaining >= speedRunnerThreshold && _correctAnswersCount == _allCountries.Count)
    {
      achievementManager.UnlockAchievement("Speed Runner");
    }

    leaderboardManager.AddPlayerEntry("YOU", _points, elapsedTime);
    mainPanel.SetActive(false);
    leaderboardPanel.SetActive(true);
    _isInputBlocked = true;
    _isGameOver = true;

    StartCoroutine(ShowGameOverPanelAfterDelay(5f, elapsedTime));
  }

  private IEnumerator ShowGameOverPanelAfterDelay(float delay, float time)
  {
    yield return new WaitForSeconds(delay);
    leaderboardPanel.SetActive(false);
    if (gameOverPanel != null)
      gameOverPanel.ShowPanel(lives, time, _points);
  }
}