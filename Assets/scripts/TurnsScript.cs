using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnsScript : MonoBehaviour
{
    public static void PassTurnToEnemy()
    {
        foreach (var enemy in Units.Enemies.ToList())
        {
            EventAggregator.EnemyTurn.Publish(enemy);
        }
        
        EventAggregator.NewTurn.Publish();
    }
}
