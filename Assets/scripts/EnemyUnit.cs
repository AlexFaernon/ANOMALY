using System;
using System.Collections.Generic;
using System.Linq;
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
        Units.Enemies.Add(enemy);

        enemy.CanMove = true;
        
        EventAggregator.UpdateHP.Subscribe(CheckDeath);
        EventAggregator.EnemyTurn.Subscribe(MakeMove);
        EventAggregator.NewTurn.Subscribe(NewTurn);
    }

    private void Start()
    {
        EventAggregator.BindHPBarToEnemy.Publish(hpBar, enemy);
    }

    private void CheckDeath(IUnit unit)
    {
        if (unit != enemy) return;

        if (enemy.HP > 0) return;
        EventAggregator.EnemyDied.Publish(enemy);
        gameObject.SetActive(false);
    }

    private void MakeMove(IEnemy other)
    {
        if (enemy != other) return;

        if (enemy.CanMove)
        {
            var character = Units.Characters.Values.OrderByDescending(character => character.HP).First();
            character.TakeDamage(1, enemy);
            Debug.Log(character);
        }
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
        EventAggregator.UpdateHP.Unsubscribe(CheckDeath);
        EventAggregator.EnemyTurn.Unsubscribe(MakeMove);
        EventAggregator.NewTurn.Unsubscribe(NewTurn);
    }
}
