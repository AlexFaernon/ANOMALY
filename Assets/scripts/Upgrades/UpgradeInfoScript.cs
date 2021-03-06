using TMPro;
using UnityEngine;

public class UpgradeInfoScript : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text upgradeLevelText;
    [SerializeField] private TMP_Text text;
    [SerializeField] private TMP_Text cooldown;
    [SerializeField] private TMP_Text cost;
    [SerializeField] private TMP_Text targetCount;
    [SerializeField] private TMP_Text emptyInfoText;
    private ICharacter character;

    private void Awake()
    {
        emptyInfoText.text = "Выберите персонажа";
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
        if (GameState.GameScene == GameScene.CharacterInfo)
        {
            EventAggregator.UpgradeCharacterSelected.Publish(CharacterInfoButton.SelectedCharacter);
        }
    }

    private void SelectCharacter(ICharacter character1)
    {
        character = character1;
        title.gameObject.SetActive(false);
        upgradeLevelText.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
        cooldown.gameObject.SetActive(false);
        cost.gameObject.SetActive(false);
        targetCount.gameObject.SetActive(false);
        emptyInfoText.gameObject.SetActive(true);
        emptyInfoText.text = "Выберите способность для улучшения";
    }

    private void SelectAbilityUpgrade(AbilityType abilityType, UpgradeLevel upgradeLevel, StatsUpgradeType statsUpgradeType, int cost1)
    {
        emptyInfoText.gameObject.SetActive(false);
        title.gameObject.SetActive(true);
        upgradeLevelText.gameObject.SetActive(true);
        text.gameObject.SetActive(true);
        upgradeLevelText.text = $"Уровень {(int)upgradeLevel / 2 + 1}";
        if ((int)upgradeLevel % 2 == 1)
        {
            cooldown.gameObject.SetActive(false);
            cost.gameObject.SetActive(false);
            targetCount.gameObject.SetActive(false);
            upgradeLevelText.gameObject.SetActive(false);
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
        title.text = ability.Name;
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
