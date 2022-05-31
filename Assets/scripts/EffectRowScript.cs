using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EffectRowScript : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text duration;
    [SerializeField] private Image icon;

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
        icon.sprite = status.Icon;
    }

    private void OnDestroy()
    {
        EventAggregator.CreateEffectRow.Unsubscribe(CreateEffectRow);
    }
}
