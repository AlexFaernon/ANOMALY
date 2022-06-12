using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentAbility : MonoBehaviour
{
    private Image image;
    private Button button;
    private void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        GetComponent<Button>().onClick.AddListener(() => EventAggregator.GetTargetsNow.Publish());
        EventAggregator.StartChooseTargets.Subscribe(OnTargetChoose);
        EventAggregator.GetTargets.Subscribe(DisableOnCast);
        gameObject.SetActive(false);
    }

    private void Start()
    {
        EventAggregator.PickTarget.Subscribe(OnTargetPick);
    }

    private void OnTargetPick(IUnit unit)
    {
        button.interactable = TargetPicker.Targets.Count != 0;
    }

    private void OnTargetChoose(IAbility ability)
    {
        gameObject.SetActive(true);
        button.interactable = false;
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
        EventAggregator.PickTarget.Unsubscribe(OnTargetPick);
    }
}
