using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUnit : MonoBehaviour
{
    [SerializeField] private TMP_Text HP;
    [SerializeField] private GameObject hpBar;
    private readonly IEnemy enemy = new Enemy();
    private bool IsPicked;

    private void Awake()
    {
        UpdateHP(enemy);
        GetComponent<Button>().onClick.AddListener(PickTarget);
        UnitsManager.Enemies.Add(enemy);

        enemy.CanMove = true;
        
        EventAggregator.UpdateHP.Subscribe(UpdateHP);
        EventAggregator.EnemyTurn.Subscribe(MakeMove);
        EventAggregator.NewTurn.Subscribe(NewTurn);
    }

    private void Start()
    {
        EventAggregator.BindHPBarToEnemy.Publish(hpBar, enemy);
    }

    private void UpdateHP(IUnit unit)
    {
        if (unit == enemy)
        {
            HP.text = enemy.HP.ToString();
        }
    }

    private void MakeMove(IEnemy other)
    {
        if (enemy != other) return;
        
        if (enemy.CanMove)
            Debug.Log("Turn");
    }

    private void NewTurn()
    {
        enemy.CanMove = true;
    }

    private void PickTarget()
    {
        if (!TargetPicker.isPicking) return;
        
        EventAggregator.PickTarget.Publish(enemy);
    }

    private void OnDestroy()
    {
        EventAggregator.UpdateHP.Unsubscribe(UpdateHP);
        EventAggregator.EnemyTurn.Unsubscribe(MakeMove);
        EventAggregator.NewTurn.Unsubscribe(NewTurn);
    }
}
