using UnityEngine;
using UnityEngine.UI;

public class TargetSquare : MonoBehaviour
{
    private bool isPicked;
    private Image image;
    private readonly Color notPickedColor = new Color(0.35f, 0.35f, 0.35f);
    private readonly Color pickedColor = new Color(0.6f, 0.6f, 0.6f);

    private void Awake()
    {
        image = GetComponent<Image>();
        image.color = notPickedColor;
        EventAggregator.ToggleTargetSquare.Subscribe(ToggleSelf);
    }

    void ToggleSelf(GameObject obj)
    {
        if (obj != gameObject) return;
        
        isPicked = !isPicked;
        image.color = isPicked ? pickedColor : notPickedColor;
    }

    private void OnDestroy()
    {
        EventAggregator.ToggleTargetSquare.Unsubscribe(ToggleSelf);
    }
}
