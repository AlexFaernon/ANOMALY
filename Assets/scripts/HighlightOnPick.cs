using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighlightOnPick : MonoBehaviour
{
    [SerializeField] private GameObject circle;

    private bool _isPicked;
    private bool IsPicked
    {
        get => _isPicked;
        set
        {
            _isPicked = value;
            circle.SetActive(value);
        }
    }

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(PickTarget);
        EventAggregator.GetTargets.Subscribe(UnPick);
        EventAggregator.DeselectTargets.Subscribe(UnPick);
        circle.SetActive(false);
    }
    
    private void PickTarget()
    {
        if (!TargetPicker.isPicking) return;

        IsPicked = !IsPicked;
        circle.SetActive(IsPicked);
    }

    private void UnPick()
    {
        IsPicked = false;
    }
    
    private void UnPick(List<IUnit> units)
    {
        UnPick();
    }

    private void OnDestroy()
    {
        EventAggregator.GetTargets.Unsubscribe(UnPick);
        EventAggregator.DeselectTargets.Unsubscribe(UnPick);
    }
}
