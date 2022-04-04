using System.Collections.Generic;
using UnityEngine;

public class TurnsScript : MonoBehaviour
{
    private static readonly HashSet<IEnemy> enemies = new HashSet<IEnemy>();

    private void Awake()
    {
        EventAggregator.NewEnemy.Subscribe(NewEnemy);
    }

    public static void PassTurnToEnemy()
    {
        foreach (var enemy in enemies)
        {
            EventAggregator.EnemyTurn.Publish(enemy);
        }
        
        EventAggregator.NewTurn.Publish();
    }

    private void NewEnemy(IEnemy enemy)
    {
        enemies.Add(enemy);
    }

    private void OnDestroy()
    {
        EventAggregator.NewEnemy.Unsubscribe(NewEnemy);
    }
}
