using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlacksmithNPC : NPCController
{
    [SerializeField] GameObject shopObject;
    [SerializeField] TMP_Text dmgCostText, hpCostText;
    public int dmgCost, hpCost;

    // Start is called before the first frame update
    void Start()
    {
        shopObject.SetActive(false);
        dmgCost = RuntimeGameSettings.Instance.GetPlayerDamage();
        hpCost = RuntimeGameSettings.Instance.GetMaxHealth() / 5; // base 200 / 5 = 40
        dmgCostText.text = dmgCost + " $";
        hpCostText.text = hpCost + " $";
    }


    public override void Interacted()
    {
        base.Interacted();
        StartCoroutine(DialogueAndShopRoutine());
    }

    IEnumerator DialogueAndShopRoutine()
    {
        //yield return null;
        //StartDialogue();
        Debug.Log("Text shown + dialogue finish is " + DialogueController.Instance.GetHasDialogueFinished());
        yield return new WaitUntil(() => DialogueController.Instance.GetHasDialogueFinished());
        OpenShop();
        Debug.Log("Text shown + dialogue finish is " + DialogueController.Instance.GetHasDialogueFinished());
    }

    public void OpenShop() // al acabar el dialeg
    {
        shopObject.SetActive(true);
        playerSettings.SetCanInteract(false);
        PlayerMovement playerMovement = FindAnyObjectByType<PlayerMovement>();
        if (playerMovement != null) 
            playerMovement.SetCanMove(false);
        ButtonController buttonController = FindAnyObjectByType<ButtonController>();
        if (buttonController != null)
            buttonController.SetCanPauseGame(false);
    }

    public void CloseShop() // close button
    {
        shopObject.SetActive(false);
        playerSettings.SetCanInteract(true);
        PlayerMovement playerMovement = FindAnyObjectByType<PlayerMovement>();
        if (playerMovement != null)
            playerMovement.SetCanMove(true);
        ButtonController buttonController = FindAnyObjectByType<ButtonController>();
        if (buttonController != null)
            buttonController.SetCanPauseGame(true);
    }

    public void BuyDamage()
    {
        if (RuntimeGameSettings.Instance.GetPlayerGold() > dmgCost)
        {
            RuntimeGameSettings.Instance.SetPlayerGold(RuntimeGameSettings.Instance.GetPlayerGold() - dmgCost);
            OverworldCurrencyController owCurrency = FindAnyObjectByType<OverworldCurrencyController>();
            if (owCurrency != null)
                owCurrency.UpdateGoldText();
            RuntimeGameSettings.Instance.SetPlayerDamage(RuntimeGameSettings.Instance.GetPlayerDamage() + 5); // +5 de dmg per cada millora
            dmgCost += 5; // el cost s'incrementa amb el dmg
            dmgCostText.text = dmgCost + " $";
        }
    }
    
    public void BuyHp()
    {
        if (RuntimeGameSettings.Instance.GetPlayerGold() > hpCost)
        {
            RuntimeGameSettings.Instance.SetPlayerGold(RuntimeGameSettings.Instance.GetPlayerGold() - hpCost);
            RuntimeGameSettings.Instance.SetMaxHealth(RuntimeGameSettings.Instance.GetMaxHealth() + 25); // +25 de hp per cada millora
            RuntimeGameSettings.Instance.SetActualHealth(RuntimeGameSettings.Instance.GetMaxHealth()); // curació
            OverworldCurrencyController owCurrency = FindAnyObjectByType<OverworldCurrencyController>();
            if (owCurrency != null)
            {
                owCurrency.UpdateGoldText();
                owCurrency.UpdateHpText();
            }
            hpCost = RuntimeGameSettings.Instance.GetMaxHealth() / 5; // el cost s'incrementa amb el dmg
            hpCostText.text = hpCost + " $";
        }
    }
}
