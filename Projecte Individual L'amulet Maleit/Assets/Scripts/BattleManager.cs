using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private int totalSpawns, earnedGold;

    [SerializeField] GameObject showVictory;
    [SerializeField] TMP_Text earnedGoldText;
    [SerializeField] MusicController musicController;

    [SerializeField] GameObject regularBackground, bossBackground;
    [SerializeField] GameObject darknessEffectObject;

    // Start is called before the first frame update
    void Awake()
    {
        //Instantiate(enemyPrefab1, enemySpawnPositions[0], Quaternion.identity);
        //Instantiate(enemyPrefab2, enemySpawnPositions[1], Quaternion.identity);
        if (RuntimeGameSettings.Instance.GetStoryProgression() != 7) { // Comprovo que sigui el BOSS (només es pot aconseguir story7 parlant amb el boss)
            regularBackground.SetActive(true);
            bossBackground.SetActive(false);

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

            totalSpawns = Random.Range((int)minSpawns, (int)maxSpawns + 1);
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

            darknessEffectObject.SetActive(false);
            if (RuntimeGameSettings.Instance.GetLastScene() == SceneController.GetCaveIndex())
            {
                earnedGold = totalSpawns * 10; // 10 de gold per cada ratpenat
                darknessEffectObject.SetActive(true);
            }
            else
                earnedGold = totalSpawns * 5;
            
        }
        else
        {
            regularBackground.SetActive(false);
            bossBackground.SetActive(true);
            earnedGold = 300;
            darknessEffectObject.SetActive(true);
        }
        earnedGoldText.text = "Has obtingut: " + earnedGold + " $"; // Mirar de posar-ho en el victory
        showVictory.SetActive(false);
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
            RuntimeGameSettings.Instance.SetPlayerGold(RuntimeGameSettings.Instance.GetPlayerGold() + earnedGold);

            //Debug.Log("Tots els enemics han sigut derrotats, VICTORIA!");
        }
        StartCoroutine(VictoryRoutine());
        //SceneController.LoadSceneByIndex(RuntimeGameSettings.Instance.lastScene);
    }

    private IEnumerator VictoryRoutine()
    {
        // mostrar or aconseguit + img de "VICTORY"
        yield return new WaitForSeconds(0.2f);

        musicController.ChangeMusicVolume(0);
        musicController.PlayVictory();
        showVictory.SetActive(true);
        yield return new WaitForSeconds(5f);
        SceneController.LoadSceneByIndex(RuntimeGameSettings.Instance.GetLastScene());
    }

    public void SetEnemiesAlive(int enemiesAlive)
    {
        this.enemiesAlive = enemiesAlive;
    }

    public int GetEnemiesAlive() { return this.enemiesAlive; }
}
