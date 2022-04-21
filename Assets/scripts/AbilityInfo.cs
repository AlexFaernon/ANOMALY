using TMPro;
using UnityEngine;

public class AbilityInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    private void Awake()
    {
        EventAggregator.ShowAbilityInfo.Subscribe(ShowInfo);
        EventAggregator.HideAbilityInfo.Subscribe(ToggleSelf);
        gameObject.SetActive(false);
    }

    void ToggleSelf()
    {
        gameObject.SetActive(false);
    }

    private void ShowInfo(IAbility ability)
    {
        gameObject.SetActive(true);
        text.text = ability.Cost.ToString();
    }

    private void OnDestroy()
    {
        EventAggregator.ShowAbilityInfo.Unsubscribe(ShowInfo);
        EventAggregator.HideAbilityInfo.Unsubscribe(ToggleSelf);
    }
}
