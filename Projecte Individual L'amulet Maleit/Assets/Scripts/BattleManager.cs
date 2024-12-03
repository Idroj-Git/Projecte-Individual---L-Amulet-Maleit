using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public float tiempoRestante = 3000f; // Posat molt alt per fer proves, CANVIAR O ELIMINAR!
    public int enemiesAlive = 0;
    private Vector2[] enemySpawnPositions = new Vector2[]
    {
        new Vector2(7, -2),
        new Vector2(5, -3),
        new Vector2(3, -4)
    };
    private float maxSpawns = 3;
    private bool victoryCalled = false;
    [SerializeField] GameObject enemyPrefab1;

    // Start is called before the first frame update
    void Awake()
    {
        int spawnCount = 0;
        while (spawnCount < maxSpawns) // max no inclós
        {
            foreach (Vector2 position in enemySpawnPositions)
            {
                if (Random.value < 0.5)
                {
                    spawnCount++;
                    Instantiate(enemyPrefab1, position, Quaternion.identity);
                }
            }
            //if (Random.value < 0.5)
            //{
            //    Instantiate(enemyPrefab1, enemySpawnPositions[1], Quaternion.identity);
            //}
            //if (Random.value < 0.5)
            //{
            //    Instantiate(enemyPrefab1, enemySpawnPositions[2], Quaternion.identity);
            //}
            //if (Random.value < 0.5)
            //{
            //    Instantiate(enemyPrefab1, enemySpawnPositions[3], Quaternion.identity);
            //}
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
        SceneController.LoadSceneByIndex(RuntimeGameSettings.Instance.lastScene);
    }

    public void SetEnemiesAlive(int enemiesAlive)
    {
        this.enemiesAlive = enemiesAlive;
    }

    public int GetEnemiesAlive() { return this.enemiesAlive; }
}
