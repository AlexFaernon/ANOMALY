using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyScript : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text price;
    [SerializeField] private GameObject boughtText;
    [SerializeField] private Button buyButton;
    [SerializeField] private List<Sprite> icons;
    private ICharacter character;

    private void Awake()
    {
        icon.gameObject.SetActive(false);
        price.gameObject.SetActive(false);
        boughtText.SetActive(false);
        EventAggregator.UpgradeAbilitySelected.Subscribe(AbilitySelected);
        EventAggregator.UpgradeCharacterSelected.Subscribe(SelectCharacter);
        EventAggregator.AbilityUpgraded.Subscribe(OnAbilityUpgrade);
    }

    private void Start()
    {
        buyButton.gameObject.SetActive(false);
    }

    private void OnAbilityUpgrade()
    {
        icon.gameObject.SetActive(false);
        price.gameObject.SetActive(false);
        boughtText.SetActive(true);
        buyButton.gameObject.SetActive(false);
    }

    private void SelectCharacter(ICharacter character1)
    {
        character = character1;
        icon.gameObject.SetActive(false);
        price.gameObject.SetActive(false);
        boughtText.SetActive(false);
        buyButton.gameObject.SetActive(false);
    }

    private void AbilitySelected(AbilityType abilityType, UpgradeLevel upgradeLevel, StatsUpgradeType statsUpgradeType, int cost)
    {
        icon.gameObject.SetActive(false);
        price.gameObject.SetActive(false);
        boughtText.SetActive(false);
        buyButton.gameObject.SetActive(false);
        buyButton.interactable = false;

        var hpUpgraded = statsUpgradeType == StatsUpgradeType.HP &&
                         (character.HP1Upgrade && upgradeLevel == UpgradeLevel.Second ||
                          character.HP2Upgrade && upgradeLevel == UpgradeLevel.Fourth);
        var mpUpgraded = statsUpgradeType == StatsUpgradeType.MP &&
                         (character.MP1Upgrade && upgradeLevel == UpgradeLevel.Second ||
                          character.MP2Upgrade && upgradeLevel == UpgradeLevel.Fourth);
        
        var ability = character.Abilities[abilityType];
        if ((int)upgradeLevel <= ability.OverallUpgradeLevel &&
            (statsUpgradeType == StatsUpgradeType.None || abilityType != AbilityType.Basic ||
             hpUpgraded || mpUpgraded))
        {
            boughtText.SetActive(true);
            return;
        }
        
        icon.gameObject.SetActive(true);
        price.gameObject.SetActive(true);
        price.text = cost.ToString();
        icon.sprite = icons[(int)abilityType];
        if ((int)upgradeLevel == ability.OverallUpgradeLevel + 1 ||
            (int)upgradeLevel <= ability.OverallUpgradeLevel && !hpUpgraded && !mpUpgraded ||
            abilityType == AbilityType.Ultimate && (int)upgradeLevel == ability.OverallUpgradeLevel + 2 ||
            statsUpgradeType == StatsUpgradeType.HPMP &&
            character.Abilities[AbilityType.First].OverallUpgradeLevel + 1 == (int)upgradeLevel &&
            character.Abilities[AbilityType.Second].OverallUpgradeLevel + 1 == (int)upgradeLevel)
        {
            buyButton.gameObject.SetActive(true);
            if (AbilityResources.Resources[abilityType] < cost)
            {
                price.color = Color.red;
                return;
            }

            price.color = Color.white;
            buyButton.interactable = true;
        }
        else
        {
            price.color = new Color(1, 1, 1, 0.7f);
        }
    }

    private void OnDestroy()
    {
        EventAggregator.UpgradeAbilitySelected.Unsubscribe(AbilitySelected);
        EventAggregator.UpgradeCharacterSelected.Unsubscribe(SelectCharacter);
        EventAggregator.AbilityUpgraded.Unsubscribe(OnAbilityUpgrade);
    }
}
