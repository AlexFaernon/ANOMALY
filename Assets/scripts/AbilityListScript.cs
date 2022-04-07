using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityListScript : MonoBehaviour
{
    [SerializeField] private GameObject abilityButtonPrefab;
    private void Awake()
    {
        EventAggregator.CreateAbilityButtons.Subscribe(CreateAbilityButtons);
        EventAggregator.ToggleOffAbilityLists.Subscribe(TurnSelfOff);
        gameObject.SetActive(false);
    }

    private void CreateAbilityButtons(GameObject other, IEnumerable<IAbility> abilities, Action<IAbility> startAbilityMethod)
    {
        if (gameObject != other) return;
        
        foreach (var ability in abilities)
        {
            var button = Instantiate(abilityButtonPrefab, transform);
            EventAggregator.BindAbilityButton.Publish(button, ability);
            button.GetComponent<Button>().onClick.AddListener(() => startAbilityMethod(ability));
        }
    }

    private void TurnSelfOff()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventAggregator.CreateAbilityButtons.Unsubscribe(CreateAbilityButtons);
        EventAggregator.ToggleOffAbilityLists.Unsubscribe(TurnSelfOff);
    }
}
