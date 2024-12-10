using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeGameSettings : MonoBehaviour
{
    public static RuntimeGameSettings Instance;

    // La idea és tenir stats/utils aquí i que es vagi actualitzant en execució. Quan l'usuari surti del joc (amb el botó de sortir) es guarden en PlayerPrefs
    private Vector2 playerLastPosition;
    private int lastScene;
    private int actualHealth; 
    private int maxHealth;
    private int playerDamage;
    private int playerGold;

    private int unsavedStoryProgression;
    public static string storyFlagName = "StoryFlag", playerLastPositionXFlag = "playerLastPosition.x", playerLastPositionYFlag = "playerLastPosition.y", lastSceneFlag = "lastScene";
    public static string actualHealthFlag = "actualHealth", maxHealthFlag = "maxHealth", playerDamageFlag = "playerDamage", playerGoldFlag = "playerGold";
    public static string StoryContainerFlag = "StoryContainer";

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

    private void Start()
    {
        lastScene = 1;
        StoryController.LoadDialoguesFromJson(StoryContainerFlag);
    }

    public void SaveGame() // Guardar dins dels playerprefs per poder tancar el joc.
    {
        if (SceneController.GetActualSceneIndex() != SceneController.GetBattleIndex())
        {
            PlayerMovement playerMovement = FindAnyObjectByType<PlayerMovement>();
            if (playerMovement != null)
            {
                playerLastPosition = playerMovement.GetPosition();
                PlayerPrefs.SetFloat(playerLastPositionXFlag, playerLastPosition.x);
                PlayerPrefs.SetFloat(playerLastPositionYFlag, playerLastPosition.y);
            }
        }

        if (SceneController.GetActualSceneIndex() != SceneController.GetBattleIndex()) // Si es l'escena de batalla s'obre l'última escena (guardada en el lloc on es fa el canvi d'escena)
        {
            lastScene = SceneController.GetActualSceneIndex();
        }
        PlayerPrefs.SetInt(lastSceneFlag, lastScene);
        PlayerPrefs.SetInt(actualHealthFlag, actualHealth);
        PlayerPrefs.SetInt(maxHealthFlag, maxHealth);
        PlayerPrefs.SetInt(playerDamageFlag, playerDamage);
        PlayerPrefs.SetInt(playerGoldFlag, playerGold);
        PlayerPrefs.SetInt(storyFlagName, unsavedStoryProgression);
        PlayerPrefs.Save(); // per si de cas
    }

    public void SetBaseStats()
    {
        playerLastPosition = new Vector2(16, 2);
        lastScene = 1;
        actualHealth = 200;
        maxHealth = 200;
        playerDamage = 30;
        playerGold = 10;
        unsavedStoryProgression = 1; // RESET HISTÒRIA
}

    public void LoadGame()
    {
        playerLastPosition = new Vector2(PlayerPrefs.GetFloat(playerLastPositionXFlag), PlayerPrefs.GetFloat(playerLastPositionYFlag));
        lastScene = PlayerPrefs.GetInt(lastSceneFlag);
        actualHealth = PlayerPrefs.GetInt(actualHealthFlag);
        maxHealth = PlayerPrefs.GetInt(maxHealthFlag);
        playerDamage = PlayerPrefs.GetInt(playerDamageFlag);
        playerGold = PlayerPrefs.GetInt(playerGoldFlag);
        unsavedStoryProgression = PlayerPrefs.GetInt(storyFlagName);
    }

    public Vector2 GetPlayerLastPostion() { return this.playerLastPosition; }
    public void SetPlayerLastPosition(Vector2 playerLastPosition)
    {
        this.playerLastPosition = playerLastPosition;
    }

    
    public int GetLastScene() { return this.lastScene; }
    public void SetLastScene(int scene)
    {
        this.lastScene = scene;
    }

    public int GetActualHealth() { return this.actualHealth; }
    public void SetActualHealth(int actualHealth)
    {
        this.actualHealth = actualHealth;
    }
    
    public int GetMaxHealth() { return this.maxHealth; }
    public void SetMaxHealth(int maxHealth)
    {
        this.maxHealth = maxHealth;
    }
    
    public int GetPlayerDamage() { return this.playerDamage; }
    public void SetPlayerDamage(int playerDamage)
    {
        this.playerDamage = playerDamage;
    }
    
    public int GetPlayerGold() { return this.playerGold; }
    public void SetPlayerGold(int playerGold)
    {
        this.playerGold = playerGold;
    }

    public int GetStoryProgression() { return this.unsavedStoryProgression; }
    public void SetStoryProgression(int unsavedStoryProgression) { this.unsavedStoryProgression = unsavedStoryProgression; }
    public void IncreaseStoryProgression() { unsavedStoryProgression += 1; }
}
