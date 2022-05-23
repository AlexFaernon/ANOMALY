using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyButtonScript : MonoBehaviour
{
    private ICharacter character;
    private IAbility ability;
    private UpgradeLevel upgradeLevel;
    private AbilityType abilityType;
    private int cost;
    private StatsUpgradeType statsUpgradeType;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(UpgradeAbility);
        EventAggregator.UpgradeCharacterSelected.Subscribe(SelectCharacter);
        EventAggregator.UpgradeAbilitySelected.Subscribe(AbilitySelected);
    }

    private void UpgradeAbility()
    {
        if ((int)upgradeLevel % 2 == 1)
        {
            if (abilityType == AbilityType.Basic)
            {
                ability.OverallUpgradeLevel = Math.Max((int)upgradeLevel, ability.OverallUpgradeLevel);
                if (statsUpgradeType == StatsUpgradeType.HP)
                {
                    if (upgradeLevel == UpgradeLevel.Second)
                    {
                        character.HP1Upgrade = true;
                    }
                    else
                    {
                        character.HP2Upgrade = true;
                    }
                }
                else
                {
                    if (upgradeLevel == UpgradeLevel.Second)
                    {
                        character.MP1Upgrade = true;
                    }
                    else
                    {
                        character.MP2Upgrade = true;
                    }
                }
            }
            else
            {
                character.Abilities[AbilityType.First].OverallUpgradeLevel = (int)upgradeLevel;
                character.Abilities[AbilityType.Second].OverallUpgradeLevel = (int)upgradeLevel;
            }
            
            StatsUpgrades.Upgrades[statsUpgradeType][upgradeLevel].Upgrade(character);
            Debug.Log($"max {character.MaxHP.ToString()}, segment {character.HPSegmentLength}, hp {character.HP}");
        }
        else
        {
            ability.OverallUpgradeLevel = (int)upgradeLevel;
        }
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

        BattleResultsSingleton.UpgradesMade++;
        EventAggregator.AbilityUpgraded.Publish();
    }
    
    private void SelectCharacter(ICharacter character1)
    {
        character = character1;
    }

    private void AbilitySelected(AbilityType abilityType, UpgradeLevel upgradeLevel1, StatsUpgradeType statsUpgradeType, int cost)
    {
        this.statsUpgradeType = statsUpgradeType;
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
