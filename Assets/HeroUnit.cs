using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroUnit : MonoBehaviour
{
    private readonly ICharacter character = new Hero();

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => EventAggregator.ChooseTargets.Publish(1));
        EventAggregator.GetTargets.Subscribe(Attack);
    }

    void Attack(List<IUnit> units)
    {
        character.Attack(units);
    }

    private void OnDestroy()
    {
        EventAggregator.GetTargets.Unsubscribe(Attack);
    }
}
