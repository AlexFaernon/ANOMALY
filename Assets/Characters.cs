using System;
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

public class Hero : ICharacter
{
    public int HP { get; private set; }
    public void TakeDamage(int damage)
    {
        HP -= damage;
        Debug.Log(HP);
    }

    public int MP { get; private set; }
    public void Attack(List<IUnit> units)
    {
        foreach (var unit in units)
        {
            unit.TakeDamage(2);
        }
    }
}

public class Enemy : IUnit
{
    public int HP { get; private set; } = 5;
    public void TakeDamage(int damage)
    {
        HP -= damage;
        Debug.Log(HP);
    }
}
