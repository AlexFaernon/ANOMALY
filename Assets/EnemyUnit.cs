using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUnit : MonoBehaviour
{
    [SerializeField] private TMP_Text HP;
    private readonly IUnit enemy = new Enemy();
    
    private void Awake()
    {
        UpdateHP(enemy);
        GetComponent<Button>().onClick.AddListener(() => EventAggregator.PickTarget.Publish(enemy));
        EventAggregator.UpdateHP.Subscribe(UpdateHP);
    }
    
    private void UpdateHP(IUnit unit)
    {
        if (unit == enemy)
        {
            HP.text = enemy.HP.ToString();
        }
    }

    private void OnDestroy()
    {
        EventAggregator.UpdateHP.Unsubscribe(UpdateHP);
    }
}
