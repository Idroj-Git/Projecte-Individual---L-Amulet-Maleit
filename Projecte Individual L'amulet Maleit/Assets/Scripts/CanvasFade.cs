using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasFade : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private bool isCanvasOpen;
    private float fadeDuration = 0.3f;

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
            //Debug.Log("Nova camera virtual" + virtualCam);
            if (virtualCam != null)
            {
                transposer = virtualCam.GetCinemachineComponent<CinemachineFramingTransposer>();
                //Debug.Log("Nou Transponder" + transposer);
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

    private void FadeInCanvas() // obrir fade, desapareix l'escena
    {
        Time.timeScale = 0;
        StartCoroutine(fadeCanvas(canvasGroup, 0f, 1f, fadeDuration));
        isCanvasOpen = true;
    }
    private void FadeOutCanvas() // tancar fade, apareix l'escena
    {
        StartCoroutine(fadeCanvas(canvasGroup, 1f, 0f, fadeDuration));
        //Time.timeScale = 1;
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

        //Debug.Log("Escena NO cargada");
        yield return new WaitForSecondsRealtime(Instance.fadeDuration + 0.2f);

        SceneManager.LoadScene(sceneIndex);

        //yield return new WaitForSeconds(2.1f); No posar res aqui sota, perquè al canviar l'escena es deixa d'executar aquest script.

        //Instance.FadeCanvas();
    }

    private static IEnumerator LoadSceneFading2()
    {
        if (Instance.transposer != null) //Assegura que hi hagi un transposer, en algunes escenes no utilitzo cinemachine per això ho necessito
        {
            Instance.CinemachineDampingChange(); // Ajusta el damping a 0 perque es mogui "instantaneament" al centre del player
        }

        // Impedeixo que el jugador i altres es puguin moure mentres s'executa el fade.
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.SetCanMove(false);
        }
        EnemyController[] enemies = FindObjectsOfType<EnemyController>();
        foreach (EnemyController enemy in enemies)
        {
            enemy.SetCanMove(false);
        }

        Time.timeScale = 1; // NECESSARI PERQUE ES MOGUI LA CAMERA!!! (el damping funciona amb deltaTime)

        yield return new WaitForSecondsRealtime(Instance.fadeDuration); // Espera a que la camera estigui a on ha d'estar

        if (Instance.transposer != null)
        {
            Instance.CinemachineDampingReset(); // Torna el damping al estat original
        }
        Time.timeScale = 0; // Torna a pausar el joc
        
        Instance.FadeCanvas(); // CANVASFADE, sempre serà el fadeout

        yield return new WaitForSecondsRealtime(Instance.fadeDuration); //Espera a que el fade s'acabi

        // Torna tot a la normalitat. 
        if (playerMovement != null)
        {
            playerMovement.SetCanMove(true);
        }
        foreach (EnemyController enemy in enemies)
        {
            enemy.SetCanMove(true);
        }
        Time.timeScale = 1;

    }

    private void CinemachineDampingChange()
    {
        //Debug.Log("Damping Changed");
        transposer.m_XDamping = 0;
        transposer.m_YDamping = 0;
        transposer.m_ZDamping = 0;
    }
    private void CinemachineDampingReset()
    {
        //Debug.Log("Damping Reset");
        transposer.m_XDamping = 1f;
        transposer.m_YDamping = 1f;
        transposer.m_ZDamping = 1f;
    }
}
