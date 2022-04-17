using System;
using TMPro;
using UnityEngine;

public class HPSegmentScript : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    private int maxHP;

    private void Awake()
    {
        EventAggregator.SetMaxHPSegment.Subscribe(SetMaxHP);
        EventAggregator.UpdateHPSegment.Subscribe(UpdateHPSegment);
    }

    private void SetMaxHP(GameObject obj, int maxHP)
    {
        if (gameObject != obj) return;

        this.maxHP = maxHP;
    }

    private void UpdateHPSegment(GameObject other, int hp)
    {
        if (gameObject != other) return;

        text.text = $"{hp}/{maxHP}";
    }

    private void OnDestroy()
    {
        EventAggregator.SetMaxHPSegment.Unsubscribe(SetMaxHP);
        EventAggregator.UpdateHPSegment.Unsubscribe(UpdateHPSegment);
    }
}
