using TMPro;
using UnityEngine;

public class AbilityInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    private void Awake()
    {
        EventAggregator.ShowAbilityInfo.Subscribe(Method);
        EventAggregator.ToggleDarken.Subscribe(ToggleSelf);
        gameObject.SetActive(false);
    }

    void ToggleSelf()
    {
        gameObject.SetActive(false);
    }
    
    void Method(IAbility ability)
    {
        gameObject.SetActive(true);
        text.text = ability.Cost.ToString();
    }

    private void OnDestroy()
    {
        EventAggregator.ShowAbilityInfo.Unsubscribe(Method);
    }
}
