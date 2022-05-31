using System;
using UnityEngine;

public class AbilitiesInfo : MonoBehaviour
{
    [SerializeField] private GameObject abilityInfoPrefab;
    private void Awake()
    {
        var character = CharacterInfoButton.SelectedCharacter;
        foreach (AbilityType abilityType in Enum.GetValues(typeof(AbilityType)))
        {
            AbilityInfo.lastAbility = character.Abilities[abilityType];
            Instantiate(abilityInfoPrefab, transform);
        }
    }
}
