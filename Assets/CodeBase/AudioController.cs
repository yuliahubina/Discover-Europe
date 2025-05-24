using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
  [SerializeField] private AudioSource audioSource;
  [SerializeField] private Slider volumeSlider;

  private void Start()
  {
    SetVolume(1.0f);

    volumeSlider.onValueChanged.AddListener(SetVolume);
  }

  private void SetVolume(float value)
  {
    audioSource.volume = value * 0.2f;
  }
}