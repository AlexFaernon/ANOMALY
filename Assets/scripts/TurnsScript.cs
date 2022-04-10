using System.Collections.Generic;
using UnityEngine;

public class TurnsScript : MonoBehaviour
{
    public static void PassTurnToEnemy()
    {
        foreach (var enemy in UnitsManager.Enemies)
        {
            EventAggregator.EnemyTurn.Publish(enemy);
        }
        
        EventAggregator.NewTurn.Publish();
    }
}
