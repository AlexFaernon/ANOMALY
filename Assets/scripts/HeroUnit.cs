using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroUnit : MonoBehaviour
{
    [SerializeField] private CharacterClass characterClass;
    [SerializeField] private GameObject abilityList;
    [SerializeField] private GameObject HPBar;
    private ICharacter character;
    private IAbility currentAbility;

    private bool CanMove
    {
        get => character.CanMove;
        set
        {
            if (value)
            {
                if (!gameObject.TryGetComponent(out Outline _))
                {
                    var outline = gameObject.AddComponent<Outline>();
                    outline.effectColor = Color.red;
                }
            }
            else
            {
                gameObject.TryGetComponent(out Outline outline);
                Destroy(outline);
            }
            character.CanMove = value;
        }
    }

    private void Awake()
    {
        character = characterClass switch
        {
            CharacterClass.Hero => new Damager(),
            CharacterClass.Medic => new Medic(),
            CharacterClass.Tank => new Tank(),
            _ => throw new ArgumentOutOfRangeException()
        };
        
        UnitsManager.Characters.Add(character);

        CanMove = true;

        GetComponent<Button>().onClick.AddListener(ToggleAbilities);

        EventAggregator.CastAbilityType.Subscribe(StartAbility);
        EventAggregator.AbilityTypeInfo.Subscribe(ShowAbilityInfoByType);
        EventAggregator.NewTurn.Subscribe(NewTurn);
    }

    private void Start()
    {
        EventAggregator.BindHPBarToCharacter.Publish(HPBar, character);
        EventAggregator.UpdateHP.Publish(character);
    }

    private void StartAbility(GameObject obj, AbilityType abilityType)
    {
        if (abilityList != obj) return;

        currentAbility = character.Abilities[abilityType];
        if (character.MP < currentAbility.Cost)
        {
            Debug.Log("No mana");
            return;
        }

        if (currentAbility.TargetCount == 0)
        {
            CastAbility(new List<IUnit> { character });
            return;
        }
        
        EventAggregator.GetTargets.Subscribe(CastAbility);
        EventAggregator.StartChooseTargets.Publish(currentAbility.TargetCount);
    }

    private void ShowAbilityInfoByType(GameObject obj, AbilityType abilityType)
    {
        if (abilityList != obj) return;
        
        EventAggregator.ShowAbilityInfo.Publish(character.Abilities[abilityType]);
    }

    private void ToggleAbilities()
    {
        if (!TargetPicker.isPicking && CanMove)
        {
            EventAggregator.ToggleOffAbilityLists.Publish();
            abilityList.SetActive(!abilityList.activeSelf);
            EventAggregator.ToggleDarkenOn.Publish();
        }
        else
        {
            EventAggregator.PickTarget.Publish(character);
        }
    }

    void CastAbility(List<IUnit> units)
    {
        currentAbility.CastAbility(units, character);
        character.MP -= currentAbility.Cost;
        CanMove = false;
        EventAggregator.GetTargets.Unsubscribe(CastAbility);
    }

    private void NewTurn()
    {
        CanMove = true;
        character.MP += 1;
    }

    private void OnDestroy()
    {
        EventAggregator.NewTurn.Unsubscribe(NewTurn);
    }

    private enum CharacterClass
    {
        Hero,
        Medic,
        Tank
    }
}

public enum AbilityType
{
    Basic,
    First,
    Second,
    Ultimate
}
