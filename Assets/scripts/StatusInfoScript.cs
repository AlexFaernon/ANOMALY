using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class StatusInfoScript : MonoBehaviour
{
    [SerializeField] private GameObject effectInfoPrefab;
    [SerializeField] private Transform parent;
    [SerializeField] private bool isForCharacters;
    private void Awake()
    {
        EventAggregator.ShowEffectsInfo.Subscribe(ShowEffectsInfo);
        EventAggregator.HideEffectsInfo.Subscribe(DeactivateSelf);
        gameObject.SetActive(false);
    }

    private void ShowEffectsInfo(IUnit unit)
    {
        if (isForCharacters != unit is ICharacter) return;
        
        gameObject.SetActive(true);
        
        foreach (var status in StatusSystem.StatusList.Where(status => status.Target == unit))
        {
            var row = Instantiate(effectInfoPrefab, parent);
            EventAggregator.CreateEffectRow.Publish(row, status);
        }
    }

    private void DeactivateSelf()
    {
        foreach (Transform transform1 in parent)
        {
            Destroy(transform1.gameObject);
        }
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventAggregator.ShowEffectsInfo.Unsubscribe(ShowEffectsInfo);
        EventAggregator.HideEffectsInfo.Unsubscribe(DeactivateSelf);
    }
}
