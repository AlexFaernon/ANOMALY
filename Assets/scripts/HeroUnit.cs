using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroUnit : MonoBehaviour
{
    [SerializeField] private CharacterClass characterClass;
    [SerializeField] private GameObject abilityList;
    [SerializeField] private TMP_Text HP;
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
            CharacterClass.Hero => new Hero(),
            CharacterClass.Medic => new Medic(),
            CharacterClass.Tank => new Tank(),
            _ => throw new ArgumentOutOfRangeException()
        };

        CanMove = true;

        UpdateHP(character);
        
        GetComponent<Button>().onClick.AddListener(ToggleAbilities);

        EventAggregator.UpdateHP.Subscribe(UpdateHP);
        EventAggregator.NewTurn.Subscribe(NewTurn);
    }

    private void Start()
    {
        EventAggregator.CreateAbilityButtons.Publish(abilityList, character.Abilities, StartAbility);
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
        CanMove = false;
        EventAggregator.GetTargets.Unsubscribe(CastAbility);
    }

    private void NewTurn()
    {
        CanMove = true;
    }

    private void OnDestroy()
    {
        EventAggregator.UpdateHP.Unsubscribe(UpdateHP);
        EventAggregator.NewTurn.Unsubscribe(NewTurn);

    }

    private enum CharacterClass
    {
        Hero,
        Medic,
        Tank
    }
}
