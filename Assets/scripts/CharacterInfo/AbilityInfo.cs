using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityInfo : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    public static IAbility lastAbility;

    private void Awake()
    {
        icon.sprite = lastAbility.Icon;
        title.text = lastAbility.Name;
        description.text = lastAbility.Description;
        EventAggregator.InfoButtonClicked.Subscribe(OnButtonClick);
    }

    private void OnButtonClick(CharacterAbilitiesStoryButton.InfoButtonType infoButtonType)
    {
        gameObject.SetActive(infoButtonType == CharacterAbilitiesStoryButton.InfoButtonType.Abilities);
    }

    private void OnDestroy()
    {
        EventAggregator.InfoButtonClicked.Unsubscribe(OnButtonClick);
    }
}
