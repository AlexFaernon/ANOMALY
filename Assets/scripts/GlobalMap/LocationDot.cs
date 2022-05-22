using System;
using UnityEngine;
using UnityEngine.UI;

public class LocationDot : MonoBehaviour
{
    private Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
        EventAggregator.LocationSwitched.Subscribe(SwitchSprite);
    }

    private void SwitchSprite(int index)
    {
        image.color = index.ToString() == name ? Color.white : Color.gray;
    }

    private void OnDestroy()
    {
        EventAggregator.LocationSwitched.Unsubscribe(SwitchSprite);
    }
}
