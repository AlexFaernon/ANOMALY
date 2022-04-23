using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUnit : MonoBehaviour
{
    [SerializeField] private GameObject hpBar;
    private readonly IEnemy enemy = new Enemy();
    private bool IsPicked;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(PickTarget);
        UnitsManager.Enemies.Add(enemy);

        enemy.CanMove = true;
        
        EventAggregator.EnemyTurn.Subscribe(MakeMove);
        EventAggregator.NewTurn.Subscribe(NewTurn);
    }

    private void Start()
    {
        EventAggregator.BindHPBarToEnemy.Publish(hpBar, enemy);
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
        EventAggregator.EnemyTurn.Unsubscribe(MakeMove);
        EventAggregator.NewTurn.Unsubscribe(NewTurn);
    }
}
