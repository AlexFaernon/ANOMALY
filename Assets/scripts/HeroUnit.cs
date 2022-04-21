using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroUnit : MonoBehaviour
{
    [SerializeField] private CharacterClass characterClass;
    [SerializeField] private GameObject HPBar;
    private ICharacter character;
    private IAbility currentAbility;

    private bool CanMove
    {
        get => character.CanMove;
        set
        {
            GetComponent<Button>().interactable = value;
            character.CanMove = value;
        }
    }

    private bool _isSelected;
    
    private bool IsSelected
    {
        get => _isSelected;
        set
        {
            var image = GetComponent<Image>();
            if (value)
            {
                EventAggregator.DeselectCharacters.Publish();
                _isSelected = true;
                image.color = Color.green;
            }
            else
            {
                _isSelected = false;
                image.color = Color.white;
            }
        }
    }

    private void Awake()
    {
        character = characterClass switch
        {
            CharacterClass.Damager => new Damager(),
            CharacterClass.Medic => new Medic(),
            CharacterClass.Tank => new Tank(),
            _ => throw new ArgumentOutOfRangeException()
        };
        
        UnitsManager.Characters.Add(character);

        CanMove = true;

        GetComponent<Button>().onClick.AddListener(SelectCharacter);

        EventAggregator.DeselectCharacters.Subscribe(Deselect);
        EventAggregator.CastAbilityType.Subscribe(StartAbility);
        EventAggregator.AbilityTypeInfo.Subscribe(ShowAbilityInfoByType);
        EventAggregator.NewTurn.Subscribe(NewTurn);
    }

    private void Start()
    {
        EventAggregator.BindHPBarToCharacter.Publish(HPBar, character);
        EventAggregator.UpdateHP.Publish(character);
    }

    private void StartAbility(AbilityType abilityType)
    {
        if (!IsSelected) return;

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

    private void ShowAbilityInfoByType(AbilityType abilityType)
    {
        if (!IsSelected) return;
        
        EventAggregator.ShowAbilityInfo.Publish(character.Abilities[abilityType]);
    }

    private void SelectCharacter()
    {
        if (!TargetPicker.isPicking && CanMove)
        {
            IsSelected = true;
            EventAggregator.ToggleAbilityList.Publish(true);
            EventAggregator.SwitchAbilities.Publish(character);
        }
        else
        {
            EventAggregator.PickTarget.Publish(character);
        }
    }

    private void Deselect()
    {
        IsSelected = false;
    }

    void CastAbility(List<IUnit> units)
    {
        currentAbility.CastAbility(units, character);
        character.MP -= currentAbility.Cost;
        IsSelected = false;
        CanMove = false;
        EventAggregator.GetTargets.Unsubscribe(CastAbility);
        EventAggregator.ToggleAbilityList.Publish(false);
    }

    private void NewTurn()
    {
        CanMove = true;
        character.MP += 1;
    }

    private void OnDestroy()
    {
        EventAggregator.DeselectCharacters.Unsubscribe(Deselect);
        EventAggregator.CastAbilityType.Unsubscribe(StartAbility);
        EventAggregator.AbilityTypeInfo.Unsubscribe(ShowAbilityInfoByType);
        EventAggregator.NewTurn.Unsubscribe(NewTurn);
    }

    private enum CharacterClass
    {
        Damager,
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
