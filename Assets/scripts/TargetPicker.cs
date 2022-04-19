using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TargetPicker : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Transform squaresParent;
    [SerializeField] private GameObject targetSquare;
    private readonly List<IUnit> targets = new List<IUnit>();
    private readonly List<GameObject> targetsSquares = new List<GameObject>();
    private int maxTargetCount;
    public static bool isPicking { get; private set; }

    private void Awake()
    {
        EventAggregator.StartChooseTargets.Subscribe(StartPicking);
        EventAggregator.PickTarget.Subscribe(ChooseTarget);
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (isPicking)
        {
            ClearTargets();
        }
        else
        {
            EventAggregator.ToggleDarkenOff.Publish();
            EventAggregator.ToggleOffAbilityLists.Publish();
        }
    }

    void StartPicking(int count)
    {
        maxTargetCount = count;

        if (maxTargetCount == 0)
        {
            EventAggregator.GetTargets.Publish(null);
        }
        
        isPicking = true;
        for (var i = 0; i < maxTargetCount; i++)
        {
            targetsSquares.Add(Instantiate(targetSquare, squaresParent));
        }
    }
    
    void ChooseTarget(IUnit unit)
    {
        if (!isPicking) return;

        if (!targets.Contains(unit))
        {
            EventAggregator.ToggleTargetSquare.Publish(targetsSquares[targets.Count]);
            targets.Add(unit);
        }
        else
        {
            EventAggregator.ToggleTargetSquare.Publish(targetsSquares[targets.Count - 1]);
            targets.Remove(unit);
        }
        
        if (targets.Count == maxTargetCount)
        {
            EventAggregator.GetTargets.Publish(targets);
            ClearTargets();
        }
    }

    private void ClearTargets()
    {
        foreach (var targetsSquare in targetsSquares)
        {
            Destroy(targetsSquare);
        }
        targetsSquares.Clear();
        
        targets.Clear();
        isPicking = false;
    }

    private void OnDestroy()
    {
        EventAggregator.StartChooseTargets.Unsubscribe(StartPicking);
        EventAggregator.PickTarget.Unsubscribe(ChooseTarget);
    }
}