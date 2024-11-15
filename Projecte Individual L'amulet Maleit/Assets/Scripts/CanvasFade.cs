using Cinemachine;
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

    [SerializeField] CinemachineVirtualCamera virtualCam;
    public CinemachineFramingTransposer transposer;

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

    private void OnEnable()
    {
        canvasGroup = GetComponent<CanvasGroup>(); // Aqui falla si no hi ha canvas group!
        virtualCam = GetComponent<CinemachineVirtualCamera>();

        if (virtualCam == null)
        {
            virtualCam = FindObjectOfType<CinemachineVirtualCamera>();
            Debug.Log("Nova camera virtual" + virtualCam);
            if (virtualCam != null)
            {
                transposer = virtualCam.GetCinemachineComponent<CinemachineFramingTransposer>();
                Debug.Log("Nou Transponder" + transposer);
            }
        }

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
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FadeInCanvas()
    {
        Time.timeScale = 0;
        StartCoroutine(fadeCanvas(canvasGroup, 0f, 1f, 0.5f));
        isCanvasOpen = true;
    }
    private void FadeOutCanvas()
    {
        StartCoroutine(fadeCanvas(canvasGroup, 1f, 0f, 0.5f));
        Time.timeScale = 1;
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
            elapsedTime += Time.unscaledDeltaTime;
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
    private static IEnumerator LoadSceneFading(int sceneIndex) // posar fading
    {
        Instance.FadeCanvas();

        Debug.Log("Escena NO cargada");
        yield return new WaitForSecondsRealtime(1.3f);

        SceneManager.LoadScene(sceneIndex);

        //yield return new WaitForSeconds(2.1f);

        //Instance.FadeCanvas();
    }

    private static IEnumerator LoadSceneFading2()
    {
        // Cambiar el damping a valores bajos inmediatamente antes de la transición
        if (Instance.transposer != null)
        {
            Instance.CinemachineDampingChange(); // Cambia el damping a 0 para eliminar el movimiento
        }

        // Pausa el tiempo para que el fade suceda sin interferencia
        yield return new WaitForSecondsRealtime(0.5f);
        Instance.FadeCanvas();

        // Pausa adicional para asegurar que el fade esté completo
        yield return new WaitForSecondsRealtime(0.5f);

        // Restablecer el damping a su valor original después del fade
        if (Instance.transposer != null)
        {
            Instance.CinemachineDampingReset();
        }

    }

    private void CinemachineDampingChange()
    {
        Debug.Log("Damping Changed");
        transposer.m_XDamping = 0;
        transposer.m_YDamping = 0;
        transposer.m_ZDamping = 0;
    }
    private void CinemachineDampingReset()
    {
        Debug.Log("Damping Reset");
        transposer.m_XDamping = 1f;
        transposer.m_YDamping = 1f;
        transposer.m_ZDamping = 1f;
    }
}
