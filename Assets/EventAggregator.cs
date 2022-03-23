using System;
using System.Collections.Generic;

public static class EventAggregator
{
    public static readonly Event<int> ChooseTargets = new Event<int>();
    public static readonly Event<IUnit> PickTarget = new Event<IUnit>();
    public static readonly Event<List<IUnit>> GetTargets = new Event<List<IUnit>>();
    public static readonly Event<IUnit> UpdateHP = new Event<IUnit>();
    public static readonly Event ToggleAbility = new Event();
}

public class Event<T>
{
    private List<Action<T>> callbacks = new List<Action<T>>();

    public void Subscribe(Action<T> action)
    {
        callbacks.Add(action);
    }
    
    public void Publish(T obj)
    {
        foreach (var action in callbacks)
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
    private List<Action> callbacks = new List<Action>();

    public void Subscribe(Action action)
    {
        callbacks.Add(action);
    }

    public void Publish()
    {
        foreach (var action in callbacks)
        {
            action();
        }
    }

    public void Unsubscribe(Action action)
    {
        callbacks.Remove(action);
    }
}
