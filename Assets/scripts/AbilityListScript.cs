using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityListScript : MonoBehaviour
{
    [SerializeField] private GameObject abilityButtonPrefab;
    private void Awake()
    {
        CreateAbilityButtons();
        EventAggregator.ToggleOffAbilityLists.Subscribe(TurnSelfOff);
        gameObject.SetActive(false);
    }

    private void CreateAbilityButtons()
    {
        foreach (AbilityType abilityInfo in Enum.GetValues(typeof(AbilityType)))
        {
            var button = Instantiate(abilityButtonPrefab, transform);
            EventAggregator.BindAbilityButton.Publish(button, abilityInfo);
        }
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
