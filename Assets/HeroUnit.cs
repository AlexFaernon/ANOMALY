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

    private void Awake()
    {
        UpdateHP(character);
        
        GetComponent<Button>().onClick.AddListener(ToggleAbilities);

        foreach (Transform child in abilityList.transform)
        {
            child.gameObject.GetComponent<Button>().onClick.AddListener(() => StartAbility(character.Ability));
        }

        EventAggregator.GetTargets.Subscribe(CastAbility);
        EventAggregator.UpdateHP.Subscribe(UpdateHP);
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
        EventAggregator.ChooseTargets.Publish(currentAbility.TargetCount);
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
    }

    private void OnDestroy()
    {
        EventAggregator.GetTargets.Unsubscribe(CastAbility);
        EventAggregator.UpdateHP.Unsubscribe(UpdateHP);
    }
}
