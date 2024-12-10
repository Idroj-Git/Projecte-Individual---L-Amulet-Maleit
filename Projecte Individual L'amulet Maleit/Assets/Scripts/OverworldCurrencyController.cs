using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OverworldCurrencyController : MonoBehaviour
{
    [SerializeField] TMP_Text hpText, goldText;

    private void Start()
    {
        UpdateHpText();
        UpdateGoldText();
    }
    public void UpdateHpText()
    {
        hpText.text = RuntimeGameSettings.Instance.GetActualHealth() + " / " + RuntimeGameSettings.Instance.GetMaxHealth();
    }
    public void UpdateGoldText()
    {
        goldText.text = RuntimeGameSettings.Instance.GetPlayerGold() + " $";
    }
}
