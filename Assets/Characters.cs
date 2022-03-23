using System;
using System.Collections.Generic;
using UnityEngine;

public class Hero : ICharacter
{
    private static int _hp = 10;
    private static int _mp = 10;

    public int HP
    {
        get => _hp;
        private set
        {
            _hp = value;
            EventAggregator.UpdateHP.Publish(this);
        }
    }
    
    public int MP
    {
        get => _mp;
        private set
        {
            _mp = value;
            throw new NotImplementedException();
        }
    }
    
    public void TakeDamage(int damage)
    {
        HP -= damage;
        Debug.Log(HP);
    }
    
    public void Attack(List<IUnit> units)
    {
        foreach (var unit in units)
        {
            unit.TakeDamage(2);
        }
    }
}