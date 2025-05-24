using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(PolygonCollider2D))]
public class Country : MonoBehaviour
{
  [Header("Base Settings")] [SerializeField]
  private string countryName;
  [SerializeField] private TextMeshPro countryNameText;

  [SerializeField] private SpriteRenderer[] countrySprites;

  [Header("Feedback Settings")] [SerializeField]
  private GameObject feedbackWindow;
  [SerializeField] private TextMeshProUGUI feedbackText;

  [SerializeField] private AudioClip correctSound;
  [SerializeField] private AudioClip wrongSound;
  [SerializeField] private float feedbackDuration = 1.5f;
  [SerializeField] private float wrongFeedbackDuration = 1f;

  [Header("Events")] public UnityEvent OnCountrySelected;

  private PolygonCollider2D _collider;
  private Camera _mainCamera;
  private AudioSource _audioSource;

  private void Awake()
  {
    _mainCamera = Camera.main;
    _collider = GetComponent<PolygonCollider2D>();
    _audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();

    ConfigureCollider();
    InitializeFeedbackWindow();
  }

  private void InitializeFeedbackWindow()
  {
    if (feedbackWindow != null)
      feedbackWindow.SetActive(false);
  }

  private void ConfigureCollider()
  {
    if (countrySprites == null || countrySprites.Length == 0)
      return;

    _collider.isTrigger = true;
    Sprite sprite = countrySprites[0].sprite;
    List<Vector2> physicsShape = new List<Vector2>();

    if (sprite.GetPhysicsShapeCount() > 0 && sprite.GetPhysicsShape(0, physicsShape) > 0)
      _collider.points = physicsShape.ToArray();
    else
      CreateFallbackCollider(sprite);
  }

  private void CreateFallbackCollider(Sprite sprite)
  {
    Vector2[] points =
    {
      new Vector2(sprite.bounds.min.x, sprite.bounds.min.y),
      new Vector2(sprite.bounds.max.x, sprite.bounds.min.y),
      new Vector2(sprite.bounds.max.x, sprite.bounds.max.y),
      new Vector2(sprite.bounds.min.x, sprite.bounds.max.y)
    };
    _collider.points = points;
  }

  private void Update()
  {
    if (Input.GetMouseButtonDown(0))
      CheckClick(Input.mousePosition);
  }

  private void CheckClick(Vector3 screenPosition)
  {
    Ray ray = _mainCamera.ScreenPointToRay(screenPosition);
    RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
    if (hit.collider == _collider)
      OnCountrySelected?.Invoke();
  }

  public void HandleCorrectAnswer(bool isSeriesBonus = false)
  {
    if (!isSeriesBonus)
    {
      StartCoroutine(ShowFeedback());
    }
    else
    {
      HideCountry();
    }
    SetCountryZPosition(-1f);
  }

  private IEnumerator ShowFeedback()
  {
    PlayFeedbackSound();

    yield return new WaitForSeconds(feedbackDuration);
  }
  
  public void HandleWrongAnswer(string expectedCountry)
  {
    StartCoroutine(ShowWrongFeedback(expectedCountry));
  }

  private IEnumerator ShowWrongFeedback(string expectedCountry)
  {
    if (wrongSound != null && _audioSource != null)
      _audioSource.PlayOneShot(wrongSound);

    if (feedbackWindow != null && feedbackText != null)
    {
      feedbackText.text = $"Oops, I think you are lost! You chose {countryName}, but you need to find {expectedCountry}.";
      feedbackWindow.SetActive(true);
    }

    yield return new WaitForSeconds(wrongFeedbackDuration);

    if (feedbackWindow != null)
      feedbackWindow.SetActive(false);
  }
  
  public void HideCountry()
  {
    _collider.enabled = false;
    foreach (var sprite in countrySprites)
    {
      // if (sprite != null)
      //   sprite.enabled = false;
    }
    
    if (countryNameText != null)
    {
      countryNameText.text = countryName;
      countryNameText.gameObject.SetActive(true);
    }
  }
  
  private void SetCountryZPosition(float z)
  {
    Vector3 pos = transform.position;
    pos.z = z;
    transform.position = pos;
  }

  private void PlayFeedbackSound()
  {
    if (correctSound != null && _audioSource != null)
      _audioSource.PlayOneShot(correctSound);
  }

  public string CountryName => countryName;
  public SpriteRenderer[] CountrySprites => countrySprites;
}