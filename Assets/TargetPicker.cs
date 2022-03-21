using System;
using System.Collections.Generic;
using UnityEngine;

public class TargetPicker : MonoBehaviour
{
    private readonly List<IUnit> targets = new List<IUnit>();
    private int targetCount = 1;
    private bool isPicking;

    private void Awake()
    {
        EventAggregator.ChooseTargets.Subscribe(StartPicking);
        EventAggregator.PickTarget.Subscribe(ChooseTarget);
    }

    void StartPicking(int count)
    {
        targetCount = count;
        isPicking = true;
        Debug.Log("StartPicking");
    }
    
    void ChooseTarget(IUnit unit)
    {
        if (!isPicking) return;

        if (!targets.Contains(unit))
        {
            targets.Add(unit);
            Debug.Log("UnitAdded");
        }
        else
        {
            targets.Remove(unit);
            Debug.Log("UnitRemoved");
        }
        
        if (targets.Count == targetCount)
        {
            isPicking = false;
            EventAggregator.GetTargets.Publish(targets);
            targets.Clear();
            Debug.Log("StopPicking");
        }
    }

    private void OnDestroy()
    {
        EventAggregator.ChooseTargets.Unsubscribe(StartPicking);
        EventAggregator.PickTarget.Unsubscribe(ChooseTarget);
    }
}
