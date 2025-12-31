using UnityEngine;
using TMPro;
using System.Collections;

public class MessagePopupController : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private float fadeDuration = 0.4f;
    [SerializeField] private float showDuration = 3.0f;
    [SerializeField, Range(0f, 1f)] private float maxAlpha = 0.9f;

    private Vector3 hiddenScale = new Vector3(0.9f, 0.9f, 1f);

    private void Awake()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        transform.localScale = hiddenScale;
        gameObject.SetActive(false);
    }

    public void Show(string message)
    {
        gameObject.SetActive(true);
        messageText.text = message;
        StopAllCoroutines();
        StartCoroutine(ShowRoutine());
    }

    public void Hide()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn()
    {
        float t = 0;
        canvasGroup.interactable = true;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float p = t / fadeDuration;

            canvasGroup.alpha = Mathf.Lerp(0f, maxAlpha, p);
            transform.localScale = Vector3.Lerp(hiddenScale, Vector3.one, p);

            yield return null;
        }

        canvasGroup.alpha = maxAlpha;
        transform.localScale = Vector3.one;
       
    }

    private IEnumerator FadeOut()
    {
        float t = 0;
        canvasGroup.interactable = false;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float p = t / fadeDuration;

            canvasGroup.alpha = Mathf.Lerp(maxAlpha, 0f, p);
            transform.localScale = Vector3.Lerp(Vector3.one, hiddenScale, p);

            yield return null;
        }

        canvasGroup.alpha = 0;
        gameObject.SetActive(false);
    }

    private IEnumerator ShowRoutine()
    {
        yield return FadeIn();

        yield return new WaitForSeconds(showDuration);

        yield return FadeOut();
    }
}