using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighlightOnPick : MonoBehaviour
{
    private Image image;
    private bool isPicked;
    private void Awake()
    {
        image = GetComponent<Image>();
        GetComponent<Button>().onClick.AddListener(PickTarget);
        EventAggregator.GetTargets.Subscribe(UnPick);
    }
    
    private void PickTarget()
    {
        if (!TargetPicker.isPicking) return;

        isPicked = !isPicked;
        image.color = isPicked ? Color.red : Color.white;
    }
    
    private void UnPick(List<IUnit> units)
    {
        image.color = Color.white;
        isPicked = false;
    }

    private void OnDestroy()
    {
        EventAggregator.GetTargets.Unsubscribe(UnPick);
    }
}
