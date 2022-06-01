using TMPro;
using UnityEngine;

public class UpgradeCharacterName : MonoBehaviour
{
    private TMP_Text characterName;

    private void Awake()
    {
        characterName = GetComponent<TMP_Text>();
        characterName.text = "";
        EventAggregator.UpgradeCharacterSelected.Subscribe(SetName);
    }

    private void SetName(ICharacter character)
    {
        characterName.text = character.Name;
    }

    private void OnDestroy()
    {
        EventAggregator.UpgradeCharacterSelected.Unsubscribe(SetName);
    }
}
