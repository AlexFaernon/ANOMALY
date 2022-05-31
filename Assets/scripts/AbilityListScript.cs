using System;
using UnityEngine;

public class AbilityListScript : MonoBehaviour
{
    [SerializeField] private GameObject abilityButtonPrefab;
    private void Awake()
    {
        CreateAbilityButtons();
        EventAggregator.ToggleAbilityList.Subscribe(ToggleSelf);
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
    
    private void ToggleSelf(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    private void OnDestroy()
    {
        EventAggregator.ToggleAbilityList.Unsubscribe(ToggleSelf);
    }
}
