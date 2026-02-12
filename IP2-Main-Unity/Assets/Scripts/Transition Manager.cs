using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance { get; private set; }
    public float fadeSpeed = 3f;
    private CanvasGroup fadeCanvasGroup;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeFadeCanvas();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeFadeCanvas()
    {
        GameObject canvasObject = new GameObject("FadeCanvas");
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;

        GameObject fadeImageObject = new GameObject("FadeImage");
        fadeImageObject.transform.SetParent(canvasObject.transform);
        Image image = fadeImageObject.AddComponent<Image>();
        image.color = Color.black;
        RectTransform rectTransform = fadeImageObject.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(1, 1);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.sizeDelta = Vector2.zero;
        fadeCanvasGroup = canvasObject.AddComponent<CanvasGroup>();
        fadeCanvasGroup.alpha = 0;

        DontDestroyOnLoad(canvasObject);
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(FadeAndLoad(sceneName));
    }
      
    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator FadeAndLoad(string sceneName)
    {
        yield return StartCoroutine(Fade(1f));
        SceneManager.LoadScene(sceneName);
        yield return StartCoroutine(Fade(0f));
    }

    private IEnumerator Fade(float targetAlpha)
    {
        fadeCanvasGroup.blocksRaycasts = true;

        while (!Mathf.Approximately(fadeCanvasGroup.alpha, targetAlpha))
        {
            fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, fadeSpeed * Time.deltaTime);
            yield return null;
        }
        fadeCanvasGroup.alpha = targetAlpha;

        fadeCanvasGroup.blocksRaycasts = false;
    }
}
