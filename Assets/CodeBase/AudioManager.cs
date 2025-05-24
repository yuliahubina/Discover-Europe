using UnityEngine;

public class AudioManager : MonoBehaviour
{
  private static AudioManager _instance;

  private void Start()
  {
    if (_instance == null)
    {
      _instance = this;
    }
    else
    {
      Destroy(gameObject);
    }

    DontDestroyOnLoad(gameObject);
  }
}