using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public float tiempoRestante = 20f;
    public int enemiesAlive = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (tiempoRestante < 0 || enemiesAlive == 0)
        {
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
        }
        SceneController.LoadScene(1);
    }

    public void SetEnemiesAlive(int enemiesAlive)
    {
        this.enemiesAlive = enemiesAlive;
    }

    public int GetEnemiesAlive() { return this.enemiesAlive; }
}
