using TMPro;
using UnityEngine;

public class StoryScript : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<TMP_Text>().text = CharacterInfoButton.SelectedCharacter.Info;
        EventAggregator.InfoButtonClicked.Subscribe(OnButtonClick);
        gameObject.SetActive(false);
    }

    private void OnButtonClick(CharacterAbilitiesStoryButton.InfoButtonType infoButtonType)
    {
        gameObject.SetActive(infoButtonType == CharacterAbilitiesStoryButton.InfoButtonType.Story);
    }

    private void OnDestroy()
    {
        EventAggregator.InfoButtonClicked.Unsubscribe(OnButtonClick);
    }
}
