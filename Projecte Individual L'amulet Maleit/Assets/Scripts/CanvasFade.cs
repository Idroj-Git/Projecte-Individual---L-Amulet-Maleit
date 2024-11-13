using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFade : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    private bool isCanvasOpen;

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        isCanvasOpen = false;
        canvasGroup.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FadeInCanvas()
    {
        StartCoroutine(fadeCanvas(canvasGroup, 0f, 1f, 5f));
        isCanvasOpen = true;
    }
    private void FadeOutCanvas()
    {
        StartCoroutine(fadeCanvas(canvasGroup, 1f, 0f, 5f));
        isCanvasOpen = false;
    }

    public void FadeCanvas()
    {
        if (isCanvasOpen)
        {
            FadeOutCanvas();
        }
        else
        {
            FadeInCanvas();
        }
    }

    public IEnumerator fadeCanvas(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        canvasGroup.alpha += startAlpha;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime/duration);

            yield return null;
        }

        yield return null;
    }
}
