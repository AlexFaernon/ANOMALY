using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroUnit : MonoBehaviour
{
    [SerializeField] private CharacterClass characterClass;
    [SerializeField] private TMP_Text HP;
    [SerializeField] private GameObject abilityList;
    private ICharacter character;
    private IAbility currentAbility;

    private bool _hasMoved;
    private bool HasMoved
    {
        get => _hasMoved;
        set
        {
            GetComponent<Button>().interactable = !value;
            _hasMoved = value;
        }
    }

    private void Awake()
    {
        switch (characterClass)
        {
            case CharacterClass.Hero:
                character = new Hero();
                break;
            case CharacterClass.Medic:
                character = new Medic();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        UpdateHP(character);
        
        GetComponent<Button>().onClick.AddListener(ToggleAbilities);

        for (var i = 0; i < 2; i++)
        {
            var child = abilityList.transform.GetChild(i);
            var ability = character.Abilities[i];
            EventAggregator.BindAbilityButton.Publish(child.gameObject, ability);
            child.gameObject.GetComponent<Button>().onClick.AddListener(() => StartAbility(ability));
        }
        abilityList.SetActive(false);
        
        EventAggregator.UpdateHP.Subscribe(UpdateHP);
        EventAggregator.NewTurn.Subscribe(NewTurn);
    }

    private void UpdateHP(IUnit unit)
    {
        if (unit == character)
        {
            HP.text = character.HP.ToString();
        }
    }

    private void StartAbility(IAbility ability)
    {
        currentAbility = ability;
        EventAggregator.GetTargets.Subscribe(CastAbility);
        EventAggregator.StartChooseTargets.Publish(currentAbility.TargetCount);
    }

    private void ToggleAbilities()
    {
        if (!TargetPicker.isPicking)
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
        currentAbility.CastAbility(units);
        HasMoved = true;
        EventAggregator.GetTargets.Unsubscribe(CastAbility);
    }

    private void NewTurn()
    {
        HasMoved = false;
    }

    private void OnDestroy()
    {
        EventAggregator.UpdateHP.Unsubscribe(UpdateHP);
        EventAggregator.NewTurn.Unsubscribe(NewTurn);

    }

    enum CharacterClass
    {
        Hero,
        Medic
    }
}
