using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUpgradeScript : MonoBehaviour
{
    [SerializeField] private AbilityType abilityType;
    [SerializeField] private UpgradeLevel upgradeLevel;
    private IAbility ability;
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        GetComponent<Button>().onClick.AddListener(OnClick);
        EventAggregator.UpgradeCharacterSelected.Subscribe(SelectCharacter);
    }

    private void OnClick()
    {
        EventAggregator.UpgradeAbilitySelected.Publish(abilityType, upgradeLevel);
    }

    private void SelectCharacter(ICharacter character)
    {
        ability = character.Abilities[abilityType];
        image.sprite = ability.Icon;
    }

    public enum UpgradeLevel
    {
        First,
        Second,
        Third
    }

    private void OnDestroy()
    {
        EventAggregator.UpgradeCharacterSelected.Unsubscribe(SelectCharacter);

    }
}
