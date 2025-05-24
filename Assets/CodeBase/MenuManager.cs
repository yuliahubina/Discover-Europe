using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
  private const string GameScene = "GameScene";
  private const string MainScene = "MainMenu";

  public void OnStartGameButtonClick() =>
    SceneManager.LoadScene(GameScene);

  public void OnBackToMainMenuButtonClick() =>
    SceneManager.LoadScene(MainScene);

  public void OnExitGameButtonClick() =>
    Application.Quit();

  public void OnRestartGameButtonClick() =>
    SceneManager.LoadScene(GameScene);
}