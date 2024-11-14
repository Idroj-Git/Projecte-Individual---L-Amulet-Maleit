using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasFade : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private bool isCanvasOpen;

    private static CanvasFade instance;
    private static bool isInitalized; //Boolea que em serveix per executar coses NOMÉS al iniciar el joc.

    // Propiedad para acceder a la instancia
    public static CanvasFade Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CanvasFade>(); // Busca el CanvasFade en l'escena si no existeix l'instancia
            }
            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>(); // Aqui falla si no hi ha canvas group!
        
        if (!isInitalized) // Es guarda una sola vegada, per evitar problemes amb el isCanvasOpen i alpha
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            isCanvasOpen = false;
            canvasGroup.alpha = 0;

            isInitalized = true;
        }
        else
        {
            isCanvasOpen = true;
            canvasGroup.alpha = 1;
            LoadSceneFadingCoroutine2();
        }

        if (instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FadeInCanvas()
    {
        StartCoroutine(fadeCanvas(canvasGroup, 0f, 1f, 2f));
        isCanvasOpen = true;
    }
    private void FadeOutCanvas()
    {
        StartCoroutine(fadeCanvas(canvasGroup, 1f, 0f, 2f));
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

    public void LoadSceneFadingCoroutine(int sceneIndex)
    {
        StartCoroutine(LoadSceneFading(sceneIndex));
    }
    public void LoadSceneFadingCoroutine2()
    {
        StartCoroutine(LoadSceneFading2());
    }
    private static IEnumerator LoadSceneFading(int sceneIndex)
    {
        Instance.FadeCanvas();

        Debug.Log("Escena NO cargada");
        yield return new WaitForSeconds(2.1f);

        SceneManager.LoadScene(sceneIndex);

        //yield return new WaitForSeconds(2.1f);

        //Instance.FadeCanvas();
    }

    private static IEnumerator LoadSceneFading2()
    {
        Instance.FadeCanvas();
        Debug.Log("Escena cargada");
        yield return new WaitForSeconds(2.1f);
    }
}
