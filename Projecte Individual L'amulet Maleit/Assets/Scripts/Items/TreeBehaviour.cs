using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TreeBehaviour : ItemController
{
    [SerializeField] Tilemap trunkMap;
    [SerializeField] Tilemap leavesMap;
    private float fadeDuration = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Interacted()
    {
        //Debug.Log("Auuu");
        ForestStoryEvents.obtainedTrees++;
        //gameObject.SetActive(false);
        StartCoroutine(TreeFading());
    }

    IEnumerator TreeFading()
    {
        float elapsedTime = 0f;

        // Obtener los colores iniciales de los Tilemaps
        Color trunkColor = trunkMap.color;
        Color leavesColor = leavesMap.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);

            // Aplicar el nuevo alfa a los colores de los Tilemaps
            trunkMap.color = new Color(trunkColor.r, trunkColor.g, trunkColor.b, alpha);
            leavesMap.color = new Color(leavesColor.r, leavesColor.g, leavesColor.b, alpha);

            yield return null;
        }

        // Asegurar que ambos Tilemaps estén completamente transparentes al final
        trunkMap.color = new Color(trunkColor.r, trunkColor.g, trunkColor.b, 0f);
        leavesMap.color = new Color(leavesColor.r, leavesColor.g, leavesColor.b, 0f);

        yield return null;
        ForestStoryEvents.obtainedTrees++;
        gameObject.SetActive(false);
    }
}
