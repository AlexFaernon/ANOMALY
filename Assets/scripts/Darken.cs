using UnityEngine;
using UnityEngine.UI;

public class Darken : MonoBehaviour
{
    private Image image;
    private bool isOn;
    
    private void Awake()
    {
        EventAggregator.ToggleDarken.Subscribe(ToggleDarken);
        image = GetComponent<Image>();
    }

    private void ToggleDarken()
    {
        image.color = isOn ? Color.clear : new Color(0.31f, 0.31f, 0.31f, 0.9f);
        isOn = !isOn;
    }

    private void OnDestroy()
    {
        EventAggregator.ToggleDarken.Unsubscribe(ToggleDarken);
    }
}
