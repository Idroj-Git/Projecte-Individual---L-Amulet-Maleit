using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class BattleManager : MonoBehaviour
{
    public float tiempoRestante = 3000f; // Posat molt alt per fer proves, CANVIAR O ELIMINAR!
    public int enemiesAlive = 0;
    private Vector2[] enemySpawnPositions = new Vector2[]
    {
        new Vector2(6, 4), //up mid
        new Vector2(8, 1.5f), //up mid
        new Vector2(5, 0), // mid mid
        new Vector2(7, -2), // etc
        new Vector2(5, -3),
        new Vector2(3, -3.7f),
        new Vector2(7.3f, -4)
    };
    private float maxSpawns = 5, minSpawns = 2;
    private bool victoryCalled = false;
    [SerializeField] GameObject enemyPrefab1;
    [SerializeField] GameObject enemyPrefab2;

    // Start is called before the first frame update
    void Awake()
    {
        GameObject enemyPrefabSpawning;
        if (RuntimeGameSettings.Instance.GetLastScene() == SceneController.GetCaveIndex()) // Cova
        {
            enemyPrefabSpawning = enemyPrefab2; // AQUI SERA EL ENEMY PREFAB 2
        }
        else
        {
            enemyPrefabSpawning = enemyPrefab1;
        }
        List<Vector2> availablePositions = new List<Vector2>(enemySpawnPositions);
        Shuffle(availablePositions);

        int totalSpawns = Random.Range((int)minSpawns, (int)maxSpawns + 1);
        int spawnCount = 0;
        while (spawnCount < totalSpawns) // max no inclós, va 1 per 1
        {
            int randomIndex = Random.Range(0, availablePositions.Count);
            Vector2 position = availablePositions[randomIndex];

            if (Random.value < 0.5f || spawnCount < minSpawns) // Així sempre apareixen 2 primer en posicions aleatories
            {
                spawnCount++;
                Instantiate(enemyPrefabSpawning, position, Quaternion.identity);
                availablePositions.RemoveAt(randomIndex);
            }
        }
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ((tiempoRestante < 0 || enemiesAlive == 0) && !victoryCalled)
        {
            victoryCalled = true;
            Victory();
        }
        else
        {
            tiempoRestante -= Time.deltaTime;
        }
    }

    private void Victory()
    {
        if (enemiesAlive == 0)
        {
            Debug.Log("Tots els enemics han sigut derrotats, VICTORIA!");
            //afegir sfx
        }
        StartCoroutine(VictoryRoutine());
        //SceneController.LoadSceneByIndex(RuntimeGameSettings.Instance.lastScene);
    }

    private IEnumerator VictoryRoutine()
    {
        // mostrar or aconseguit + img de "VICTORY"
        yield return new WaitForSeconds(2);
        SceneController.LoadSceneByIndex(RuntimeGameSettings.Instance.GetLastScene());
    }

    public void SetEnemiesAlive(int enemiesAlive)
    {
        this.enemiesAlive = enemiesAlive;
    }

    public int GetEnemiesAlive() { return this.enemiesAlive; }
}
