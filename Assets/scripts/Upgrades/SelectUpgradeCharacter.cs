using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectUpgradeCharacter : MonoBehaviour
{
    [SerializeField] private CharacterClass characterClass;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
            EventAggregator.UpgradeCharacterSelected.Publish(Units.Characters[characterClass]));
    }
}
