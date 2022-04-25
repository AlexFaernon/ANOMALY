using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EffectRowScript : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text duration;

    private void Awake()
    {
        EventAggregator.CreateEffectRow.Subscribe(CreateEffectRow);
    }

    private void CreateEffectRow(GameObject other, Status status)
    {
        if (gameObject != other) return;

        title.text = status.Name;
        description.text = status.Description;
        duration.text = status.Duration.ToString();
    }

    private void OnDestroy()
    {
        EventAggregator.CreateEffectRow.Unsubscribe(CreateEffectRow);
    }
}
