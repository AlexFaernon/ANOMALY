using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeInfoScript : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text upgradeLevelText;
    [SerializeField] private TMP_Text text;
    [SerializeField] private TMP_Text cooldown;
    [SerializeField] private TMP_Text cost;
    [SerializeField] private TMP_Text targetCount;
    [SerializeField] private TMP_Text upgradeCostText;
    [SerializeField] private Image upgradeIcon;
    [SerializeField] private List<Sprite> icons;
    private ICharacter character;

    private void Awake()
    {
        EventAggregator.UpgradeCharacterSelected.Subscribe(SelectCharacter);
        EventAggregator.UpgradeAbilitySelected.Subscribe(SelectUpgrade);
    }

    private void SelectCharacter(ICharacter character1)
    {
        character = character1;
    }

    private void SelectUpgrade(AbilityType abilityType, AbilityUpgradeScript.UpgradeLevel upgradeLevel, int upgradeCost)
    {
        var ability = character.Abilities[abilityType];
        var level = ability.UpgradeLevel;
        ability.UpgradeLevel = (int)upgradeLevel;
        title.text = ability.ToString();
        upgradeLevelText.text = $"Уровень {(int)upgradeLevel + 1}";
        text.text = ability.Description;
        cost.text = ability.Cost.ToString();
        cooldown.text = ability.Cooldown.ToString();
        targetCount.text = ability.TargetCount.ToString();
        upgradeCostText.text = upgradeCost.ToString();
        upgradeIcon.sprite = icons[(int)abilityType];
        ability.UpgradeLevel = level;
    }

    private void OnDestroy()
    {
        EventAggregator.UpgradeCharacterSelected.Unsubscribe(SelectCharacter);
        EventAggregator.UpgradeAbilitySelected.Unsubscribe(SelectUpgrade);
    }
}
