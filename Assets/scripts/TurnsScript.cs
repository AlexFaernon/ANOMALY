using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnsScript : MonoBehaviour
{
    public static bool enemyMoved;

    private void Awake()
    {
        EventAggregator.EnemyTurn.Subscribe(PassTurnToEnemy);
    }

    private void PassTurnToEnemy()
    {
        foreach (var character in Units.Characters.Values)
        {
            character.CanMove = false;
        }
        StartCoroutine(EnemyMoves());
    }
    
    private IEnumerator EnemyMoves()
    {
        foreach (var enemy in Units.Enemies.ToList())
        {
            EventAggregator.EnemyMove.Publish(enemy);
            yield return new WaitUntil(() => enemyMoved);
            enemyMoved = false;
        }
        
        EventAggregator.NewTurn.Publish();
    }
}
