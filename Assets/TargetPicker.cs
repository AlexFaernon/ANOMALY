using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TargetPicker : MonoBehaviour, IPointerDownHandler
{
    private readonly List<IUnit> targets = new List<IUnit>();
    private int targetCount = 1;
    public static bool isPicking { get; private set; }

    private void Awake()
    {
        EventAggregator.ChooseTargets.Subscribe(StartPicking);
        EventAggregator.PickTarget.Subscribe(ChooseTarget);
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        targets.Clear();
        isPicking = false;
        Debug.Log("StopPicking");
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
