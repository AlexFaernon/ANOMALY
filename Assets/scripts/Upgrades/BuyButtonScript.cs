using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyButtonScript : MonoBehaviour
{
    private ICharacter character;
    private IAbility ability;
    private AbilityUpgradeScript.UpgradeLevel upgradeLevel;
    private AbilityType abilityType;
    private int cost;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(UpgradeAbility);
        EventAggregator.UpgradeCharacterSelected.Subscribe(SelectCharacter);
        EventAggregator.UpgradeAbilitySelected.Subscribe(AbilitySelected);
    }

    private void UpgradeAbility()
    {
        ability.UpgradeLevel = (int)upgradeLevel;
        switch (abilityType)
        {
            case AbilityType.Basic:
                AbilityResources.BasicTokens -= cost;
                break;
            case AbilityType.First:
                AbilityResources.AdvancedTokens -= cost;
                break;
            case AbilityType.Second:
                AbilityResources.AdvancedTokens -= cost;
                break;
            case AbilityType.Ultimate:
                AbilityResources.UltimateTokens -= cost;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        EventAggregator.AbilityUpgraded.Publish();
    }
    
    private void SelectCharacter(ICharacter character1)
    {
        character = character1;
    }

    private void AbilitySelected(AbilityType abilityType, AbilityUpgradeScript.UpgradeLevel upgradeLevel1, int cost)
    {
        this.cost = cost;
        this.abilityType = abilityType;
        ability = character.Abilities[abilityType];
        upgradeLevel = upgradeLevel1;
    }

    private void OnDestroy()
    {
        EventAggregator.UpgradeCharacterSelected.Unsubscribe(SelectCharacter);
        EventAggregator.UpgradeAbilitySelected.Unsubscribe(AbilitySelected);
    }
}
