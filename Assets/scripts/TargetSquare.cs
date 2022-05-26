using UnityEngine;
using UnityEngine.UI;

public class TargetSquare : MonoBehaviour
{
    [SerializeField] private Sprite unpickedSprite;
    [SerializeField] private Sprite pickedSprite;
    private bool isPicked;
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        image.sprite = unpickedSprite;
        EventAggregator.ToggleTargetSquare.Subscribe(ToggleSelf);
    }

    private void ToggleSelf(GameObject obj)
    {
        if (obj != gameObject) return;
        
        isPicked = !isPicked;
        image.sprite = isPicked ? pickedSprite : unpickedSprite;
    }

    private void OnDestroy()
    {
        EventAggregator.ToggleTargetSquare.Unsubscribe(ToggleSelf);
    }
}
