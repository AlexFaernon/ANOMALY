using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroUnit : MonoBehaviour
{
    [SerializeField] private TMP_Text HP;
    [SerializeField] private GameObject abilityList;
    private readonly ICharacter character = new Hero();
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

        EventAggregator.GetTargets.Subscribe(CastAbility);
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
        EventAggregator.StartChooseTargets.Publish(currentAbility.TargetCount);
    }

    private void ToggleAbilities()
    {
        if (!TargetPicker.isPicking)
        {
            abilityList.SetActive(!abilityList.activeSelf);
            EventAggregator.ToggleDarken.Publish();
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
    }

    private void NewTurn()
    {
        HasMoved = false;
    }

    private void OnDestroy()
    {
        EventAggregator.GetTargets.Unsubscribe(CastAbility);
        EventAggregator.UpdateHP.Unsubscribe(UpdateHP);
        EventAggregator.NewTurn.Unsubscribe(NewTurn);

    }
}
