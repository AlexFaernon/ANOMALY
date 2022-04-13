using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class EventAggregator
{
    public static readonly Event<int> StartChooseTargets = new Event<int>();
    public static readonly Event<IUnit> PickTarget = new Event<IUnit>();
    public static readonly Event<List<IUnit>> GetTargets = new Event<List<IUnit>>();
    public static readonly Event<IUnit> UpdateHP = new Event<IUnit>();
    public static readonly Event ToggleDarkenOn = new Event();
    public static readonly Event ToggleDarkenOff = new Event();
    public static readonly Event<GameObject> ToggleTargetSquare = new Event<GameObject>();
    public static readonly Event<IAbility> ShowAbilityInfo = new Event<IAbility>();
    public static readonly Event<GameObject, AbilityType> BindAbilityButton = new Event<GameObject, AbilityType>();
    public static readonly Event NewTurn = new Event();
    public static readonly Event<IEnemy> EnemyTurn = new Event<IEnemy>();
    public static readonly Event ToggleOffAbilityLists = new Event();
    public static readonly Event<GameObject, AbilityType> CastAbilityType = new Event<GameObject, AbilityType>();
    public static readonly Event<GameObject, AbilityType> AbilityTypeInfo = new Event<GameObject, AbilityType>();
}

public class Event<T>
{
    private readonly List<Action<T>> callbacks = new List<Action<T>>();

    public void Subscribe(Action<T> action)
    {
        callbacks.Add(action);
    }
    
    public void Publish(T obj)
    {
        foreach (var action in callbacks.ToList())
        {
            action(obj);
        }
    }

    public void Unsubscribe(Action<T> action)
    {
        callbacks.Remove(action);
    }
}

public class Event
{
    private readonly List<Action> callbacks = new List<Action>();

    public void Subscribe(Action action)
    {
        callbacks.Add(action);
    }

    public void Publish()
    {
        foreach (var action in callbacks.ToList())
        {
            action();
        }
    }

    public void Unsubscribe(Action action)
    {
        callbacks.Remove(action);
    }
}

public class Event<T1, T2>
{
    private readonly List<Action<T1 ,T2>> callbacks = new List<Action<T1 ,T2>>();

    public void Subscribe(Action<T1 ,T2> action)
    {
        callbacks.Add(action);
    }

    public void Publish(T1 t1, T2 t2)
    {
        foreach (var action in callbacks.ToList())
        {
            action(t1, t2);
        }
    }

    public void Unsubscribe(Action<T1 ,T2> action)
    {
        callbacks.Remove(action);
    }
}

public class Event<T1, T2, T3>
{
    private readonly List<Action<T1 ,T2, T3>> callbacks = new List<Action<T1 ,T2, T3>>();

    public void Subscribe(Action<T1 ,T2, T3> action)
    {
        callbacks.Add(action);
    }

    public void Publish(T1 t1, T2 t2, T3 t3)
    {
        foreach (var action in callbacks.ToList())
        {
            action(t1, t2, t3);
        }
    }

    public void Unsubscribe(Action<T1 ,T2, T3> action)
    {
        callbacks.Remove(action);
    }
}
