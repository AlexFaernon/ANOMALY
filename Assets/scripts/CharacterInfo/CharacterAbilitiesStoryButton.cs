using UnityEngine;
using UnityEngine.UI;

public class CharacterAbilitiesStoryButton : MonoBehaviour
{
    [SerializeField] private InfoButtonType buttonType;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => EventAggregator.InfoButtonClicked.Publish(buttonType));
        EventAggregator.InfoButtonClicked.Subscribe(OnButtonClick);
        button.interactable = buttonType != InfoButtonType.Abilities;
    }

    private void OnButtonClick(InfoButtonType infoButtonType)
    {
        button.interactable = buttonType != infoButtonType;
    }

    private void OnDestroy()
    {
        EventAggregator.InfoButtonClicked.Unsubscribe(OnButtonClick);
    }
    
    public enum InfoButtonType
    {
        Abilities,
        Story
    }
}
