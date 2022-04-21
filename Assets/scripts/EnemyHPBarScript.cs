using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBarScript : MonoBehaviour
{
    [SerializeField] private GameObject hpSquarePrefab;
    private IEnemy enemy;
    private readonly List<GameObject> hpSquares = new List<GameObject>();

    private void Awake()
    {
        EventAggregator.BindHPBarToEnemy.Subscribe(BindHPBarToEnemy);
        EventAggregator.UpdateHP.Subscribe(UpdateHP);
    }

    private void BindHPBarToEnemy(GameObject obj, IEnemy enemy)
    {
        if (gameObject != obj) return;

        this.enemy = enemy;
        CreateHPBar();
    }

    private void CreateHPBar()
    {
        var hp = enemy.HP;

        for (var i = 0; i < hp; i++)
        {
            hpSquares.Add(Instantiate(hpSquarePrefab, transform));
        }
    }

    private void UpdateHP(IUnit unit)
    {
        if (enemy != unit) return;

        var hp = enemy.HP;
        foreach (var hpSquare in hpSquares)
        {
            if (hp > 0)
            {
                hp--;
                hpSquare.GetComponent<Image>().color = Color.white;
            }
            else
            {
                hpSquare.GetComponent<Image>().color = Color.gray;
            }
        }
    }

    private void OnDestroy()
    {
        EventAggregator.BindHPBarToEnemy.Unsubscribe(BindHPBarToEnemy);
        EventAggregator.UpdateHP.Unsubscribe(UpdateHP);
    }
}
