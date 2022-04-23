using TMPro;
using UnityEngine;

public class AbilityInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text Title;
    [SerializeField] private TMP_Text Description;
    [SerializeField] private TMP_Text Cooldown;
    [SerializeField] private TMP_Text Cost;

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
        Title.text = "заглушка";
        Description.text = ability.Description;
        Cooldown.text = ability.Cooldown.ToString();
        Cost.text = ability.Cost.ToString();
    }

    private void OnDestroy()
    {
        EventAggregator.ShowAbilityInfo.Unsubscribe(ShowInfo);
        EventAggregator.HideAbilityInfo.Unsubscribe(ToggleSelf);
    }
}
