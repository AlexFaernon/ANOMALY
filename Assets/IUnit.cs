using System.Collections.Generic;
using UnityEngine;

public interface IUnit
{
    public int HP { get; }

    public void TakeDamage(int damage);
}

public interface ICharacter : IUnit
{
    public int MP { get; }

    public void Attack(List<IUnit> unit);
}

public class Enemy : IUnit
{
    private int _hp = 5;

    public int HP
    {
        get => _hp;
        private set
        {
            _hp = value;
            EventAggregator.UpdateHP.Publish(this);
        }
    }
    public void TakeDamage(int damage)
    {
        HP -= damage;
        Debug.Log(HP);
    }
}
