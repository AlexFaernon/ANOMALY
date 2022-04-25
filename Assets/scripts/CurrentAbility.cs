using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentAbility : MonoBehaviour
{
    private Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
        GetComponent<Button>().onClick.AddListener(() => EventAggregator.GetTargetsNow.Publish());
        EventAggregator.StartChooseTargets.Subscribe(OnTargetChoose);
        EventAggregator.GetTargets.Subscribe(DisableOnCast);
        gameObject.SetActive(false);
    }

    private void OnTargetChoose(IAbility ability)
    {
        gameObject.SetActive(true);
        image.sprite = ability.Icon;
    }
    
    private void DisableOnCast(List<IUnit> units)
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventAggregator.StartChooseTargets.Unsubscribe(OnTargetChoose);
        EventAggregator.GetTargets.Unsubscribe(DisableOnCast);
    }
}
