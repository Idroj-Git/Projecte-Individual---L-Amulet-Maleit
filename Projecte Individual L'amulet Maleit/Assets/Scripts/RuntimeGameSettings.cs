using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeGameSettings : MonoBehaviour
{
    public static RuntimeGameSettings Instance;

    public Vector2 playerLastPosition;

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

    // getter / setter de playerLastPosition
}
