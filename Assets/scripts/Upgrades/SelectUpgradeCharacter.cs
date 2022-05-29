using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectUpgradeCharacter : MonoBehaviour
{
    [SerializeField] private CharacterClass characterClass;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        EventAggregator.UpgradeCharacterSelected.Subscribe(OnSwitchingCharacter);
    }

    private void OnClick()
    {
        EventAggregator.UpgradeCharacterSelected.Publish(Units.Characters[characterClass]);
        button.interactable = false;
    }
    
    private void OnSwitchingCharacter(ICharacter obj)
    {
        button.interactable = true;
    }

    private void OnDestroy()
    {
        EventAggregator.UpgradeCharacterSelected.Unsubscribe(OnSwitchingCharacter);
    }
}
