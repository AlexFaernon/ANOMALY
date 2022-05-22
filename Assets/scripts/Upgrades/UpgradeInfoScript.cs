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
    private ICharacter character;

    private void Awake()
    {
        title.text = "";
        upgradeLevelText.text = "";
        text.text = "";
        cooldown.gameObject.SetActive(false);
        cost.gameObject.SetActive(false);
        targetCount.gameObject.SetActive(false);
        EventAggregator.UpgradeCharacterSelected.Subscribe(SelectCharacter);
        EventAggregator.UpgradeAbilitySelected.Subscribe(SelectAbilityUpgrade);
    }

    private void Start()
    {
        if (CurrentGameScene.GameScene == GameScene.CharacterInfo)
        {
            EventAggregator.UpgradeCharacterSelected.Publish(CharacterInfoButton.SelectedCharacter);
        }
    }

    private void SelectCharacter(ICharacter character1)
    {
        character = character1;
    }

    private void SelectAbilityUpgrade(AbilityType abilityType, UpgradeLevel upgradeLevel, StatsUpgradeType statsUpgradeType, int cost1)
    {
        upgradeLevelText.text = $"Уровень {(int)upgradeLevel + 1}";
        if ((int)upgradeLevel % 2 == 1)
        {
            cooldown.gameObject.SetActive(false);
            cost.gameObject.SetActive(false);
            targetCount.gameObject.SetActive(false);
            var upgrade = StatsUpgrades.Upgrades[statsUpgradeType][upgradeLevel];
            title.text = "Улучшение характеристик";
            text.text = upgrade.Description;
            return;
        }
        cooldown.gameObject.SetActive(true);
        cost.gameObject.SetActive(true);
        targetCount.gameObject.SetActive(true);
        var ability = character.Abilities[abilityType];
        var level = ability.OverallUpgradeLevel;
        ability.OverallUpgradeLevel = (int)upgradeLevel;
        title.text = ability.ToString();
        text.text = ability.Description;
        cost.text = ability.Cost.ToString();
        cooldown.text = ability.Cooldown.ToString();
        targetCount.text = ability.TargetCount.ToString();
        ability.OverallUpgradeLevel = level;
    }

    private void OnDestroy()
    {
        EventAggregator.UpgradeCharacterSelected.Unsubscribe(SelectCharacter);
        EventAggregator.UpgradeAbilitySelected.Unsubscribe(SelectAbilityUpgrade);
    }
}
