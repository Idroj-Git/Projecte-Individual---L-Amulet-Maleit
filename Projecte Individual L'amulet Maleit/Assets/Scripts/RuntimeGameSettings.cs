using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeGameSettings : MonoBehaviour
{
    public static RuntimeGameSettings Instance;

    public Vector2 playerLastPosition; // POSAR PRIVATE I ARREGLAR ERRORS
    public int lastScene;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetPlayerLastPosition(Vector2 playerLastPosition)
    {
        this.playerLastPosition = playerLastPosition;
    }

    public Vector2 GetPlayerLastPostion()
    {
        return this.playerLastPosition;
    }
    
    public int GetLastScene()
    {
        return this.lastScene;
    }
    public void SetLastScene(int scene)
    {
        this.lastScene = scene;
    }
}
