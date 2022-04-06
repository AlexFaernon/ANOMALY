using System;
using UnityEngine;

public class AbilityListScript : MonoBehaviour
{
    private void Awake()
    {
        EventAggregator.ToggleOffAbilityLists.Subscribe(TurnSelfOff);
    }

    private void TurnSelfOff()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventAggregator.ToggleOffAbilityLists.Unsubscribe(TurnSelfOff);
    }
}
