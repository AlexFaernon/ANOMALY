using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class TargetPicker : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Transform squaresParent;
    [SerializeField] private GameObject targetSquare;
    [SerializeField] private GameObject abilityIcon;
    public static readonly List<IUnit> Targets = new List<IUnit>();
    private readonly List<GameObject> targetsSquares = new List<GameObject>();
    private int maxTargetCount;
    public static bool isPicking { get; private set; }

    private void Awake()
    {
        EventAggregator.StartChooseTargets.Subscribe(StartPicking);
        EventAggregator.PickTarget.Subscribe(ChooseTarget);
        EventAggregator.GetTargetsNow.Subscribe(GetTargetsNow);
        EventAggregator.NewTurn.Subscribe(ResetOnTurn);
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isPicking) return;
        
        ClearTargets();
        EventAggregator.DeselectCharacters.Publish();
    }

    void StartPicking(IAbility ability)
    {
        maxTargetCount = ability.TargetCount;

        if (maxTargetCount == 0)
        {
            EventAggregator.GetTargets.Publish(null);
        }
        
        isPicking = true;
        for (var i = 0; i < maxTargetCount; i++)
        {
            targetsSquares.Add(Instantiate(targetSquare, squaresParent));
            targetsSquares.Last().name = (i + 1).ToString();
        }
    }
    
    void ChooseTarget(IUnit unit)
    {
        if (!isPicking) return;

        if (!Targets.Contains(unit))
        {
            EventAggregator.ToggleTargetSquare.Publish(targetsSquares[Targets.Count]);
            Targets.Add(unit);
        }
        else
        {
            EventAggregator.ToggleTargetSquare.Publish(targetsSquares[Targets.Count - 1]);
            Targets.Remove(unit);
        }
        
        if (Targets.Count == maxTargetCount)
        {
            EventAggregator.GetTargets.Publish(Targets);
            ClearTargets();
        }
    }

    private void GetTargetsNow()
    {
        if (Targets.Count == 0) return;
        
        EventAggregator.GetTargets.Publish(Targets);
        ClearTargets();
    }

    private void ResetOnTurn()
    {
        if (!isPicking) return;
        
        ClearTargets();
        EventAggregator.DeselectCharacters.Publish();
        EventAggregator.ToggleAbilityList.Publish(false);
    }
    
    private void ClearTargets()
    {
        foreach (var targetsSquare in targetsSquares)
        {
            Destroy(targetsSquare);
        }
        targetsSquares.Clear();
        
        Targets.Clear();
        isPicking = false;
        EventAggregator.DeselectTargets.Publish();
        abilityIcon.SetActive(false);
    }

    private void OnDestroy()
    {
        EventAggregator.StartChooseTargets.Unsubscribe(StartPicking);
        EventAggregator.PickTarget.Unsubscribe(ChooseTarget);
        EventAggregator.GetTargetsNow.Unsubscribe(GetTargetsNow);
        EventAggregator.NewTurn.Unsubscribe(ResetOnTurn);
    }
}
