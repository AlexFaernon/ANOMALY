using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeInfoScript : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text text;
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

    private void SelectUpgrade(AbilityType abilityType, AbilityUpgradeScript.UpgradeLevel upgradeLevel)
    {
        
    }

    private void OnDestroy()
    {
        EventAggregator.UpgradeCharacterSelected.Unsubscribe(SelectCharacter);
        EventAggregator.UpgradeAbilitySelected.Unsubscribe(SelectUpgrade);
    }
}
