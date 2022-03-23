using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroUnit : MonoBehaviour
{
    [SerializeField] private TMP_Text HP;
    [SerializeField] private GameObject abilityList;
    private readonly ICharacter character = new Hero();

    private void Awake()
    {
        UpdateHP(character);
        
        GetComponent<Button>().onClick.AddListener(ToggleAbilities);

        foreach (Transform child in abilityList.transform)
        {
            child.gameObject.GetComponent<Button>().onClick.AddListener(() => EventAggregator.ChooseTargets.Publish(1));
        }

        EventAggregator.GetTargets.Subscribe(Attack);
        EventAggregator.UpdateHP.Subscribe(UpdateHP);
    }

    private void UpdateHP(IUnit unit)
    {
        if (unit == character)
        {
            HP.text = character.HP.ToString();
        }
    }

    private void ToggleAbilities()
    {
        abilityList.SetActive(!abilityList.activeSelf);
    }

    void Attack(List<IUnit> units)
    {
        character.Attack(units);
    }

    private void OnDestroy()
    {
        EventAggregator.GetTargets.Unsubscribe(Attack);
        EventAggregator.UpdateHP.Unsubscribe(UpdateHP);
    }
}
