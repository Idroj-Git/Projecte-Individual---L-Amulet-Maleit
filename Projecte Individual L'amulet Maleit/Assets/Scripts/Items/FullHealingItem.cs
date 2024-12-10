using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullHealingItem : ItemController
{
    public override void Interacted()
    {
        FullHeal();
    }
    private void FullHeal()
    {
        RuntimeGameSettings.Instance.SetActualHealth(RuntimeGameSettings.Instance.GetMaxHealth());
        OverworldCurrencyController owCurrency = FindAnyObjectByType<OverworldCurrencyController>();
        if (owCurrency != null)
            owCurrency.UpdateHpText();
    }
}
